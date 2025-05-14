using Microsoft.Extensions.Options;
using Win2Mqtt.HomeAssistant;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.Service
{
    public sealed class Win2MqttService(
        IMqttConnectionManager mqttConnectionManager,
        IMqttSubscriber mqttSubscriber,
        IMqttPublisher mqttPublisher,
        ISystemMetricsCollector sensorDataCollector,
        IIncomingMessagesProcessor incomingMessagesProcessor,
        IHomeAssistantPublisher haPublisher,
        IOptions<Win2MqttOptions> options,
        ILogger<Win2MqttService> logger)
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly static SemaphoreSlim _semaphore = new(1, 1);


        public async Task StartAsync(CancellationToken stoppingToken)
        {
            // Connect to MQTT broker
            await ConnectToMqttBrokerAsync(stoppingToken);

            // Subscribe to incoming messages and  process them with IIncomingMessagesProcessor.ProcessMessageAsync()
            await SubscribeToIncomingMessagesAsync(stoppingToken);

            // Publish Home Assistant discovery messages
            await haPublisher.PublishSensorsDiscoveryAsync(stoppingToken);

            // Publish online status
            await haPublisher.NotifyOnlineStatus(stoppingToken);
        }

        public async Task CollectAndPublish(CancellationToken stoppingToken)
        {

            // Allow only one thread collecting system information
            await _semaphore.WaitAsync(stoppingToken);
            try
            {
                // Collect system information
                var sensorsData = await sensorDataCollector.CollectAsync();

                // Publish collected data
                foreach (var sensorData in sensorsData)
                {
                    try
                    {
                        var sensorTopic = $"{Constants.ServiceBaseTopic}/{_options.MachineIdentifier}/{sensorData.Key}";
                        await mqttPublisher.PublishAsync(sensorTopic, sensorData.Value, false, cancellationToken: stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Failed to publish sensor {Sensor}", sensorData.Key);
                    }
                }
                logger.LogDebug("{sensorCount} sensors published", sensorsData.Count);
            }
            finally
            {
                _semaphore.Release();
            }
            await Task.Delay(TimeSpan.FromSeconds(_options.TimerInterval), stoppingToken);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Publish offline status
            await haPublisher.NotifyOfflineStatus(cancellationToken);
            await mqttConnectionManager.DisconnectAsync(cancellationToken);

        }

        public async Task ConnectToMqttBrokerAsync(CancellationToken stoppingToken)
        {
            while (!(await mqttConnectionManager.ConnectAsync(stoppingToken)))
            {
                logger.LogWarning("MQTT connection failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        public async Task SubscribeToIncomingMessagesAsync(CancellationToken stoppingToken)
        {
            while (!(await mqttSubscriber.SubscribeAsync(incomingMessagesProcessor.ProcessMessageAsync, stoppingToken)))
            {
                logger.LogWarning("MQTT subscription failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

}
