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
        ISensorFactory sensorFactory,
        ISensorValueFormatter sensorValueFormatter,
        IIncomingMessagesProcessor incomingMessagesProcessor,
        IHomeAssistantPublisher haPublisher,
        IOptions<Win2MqttOptions> options,
        ILogger<Win2MqttService> logger)
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly static SemaphoreSlim _semaphore = new(1, 1);
        private readonly IEnumerable<ISensorWrapper> _sensors = sensorFactory.GetEnabledSensors();
        
        private readonly object DeviceInfo = HomeAssistantDiscoveryHelper.GetHADeviceInfo(options.Value.DeviceUniqueId);

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            // Connect to MQTT broker
            await ConnectToMqttBrokerAsync(stoppingToken);

            // Subscribe to incoming messages and  process them with IIncomingMessagesProcessor.ProcessMessageAsync()
            await SubscribeToIncomingMessagesAsync(stoppingToken);

            // Publish Home Assistant discovery messages
            foreach (var sensor in _sensors)
            {
                await mqttPublisher.PublishAsync(
                    HomeAssistantDiscoveryHelper.GetHADiscoveryMessage(options.Value.MqttBaseTopic, sensor.Metadata, DeviceInfo), 
                    retain: true, 
                    cancellationToken: stoppingToken);

                logger.LogInformation("Published HA binary_sensor config for {sensor}", sensor.Metadata.Key);

            }

            // Publish online status
            await haPublisher.NotifyOnlineStatus(stoppingToken);
        }

        public async Task CollectAndPublish(CancellationToken stoppingToken)
        {

            // Allow only one thread collecting system information
            await _semaphore.WaitAsync(stoppingToken);
            try
            {
                // Collect system information / Publish collected data
                foreach (var sensor in _sensors)
                {
                    try
                    {
                        if(sensor.Metadata.SensorStateTopic == null)
                        {
                            logger.LogWarning("Sensor {Sensor} has no state topic", sensor.Metadata.Key);
                            continue;
                        }
                        var value = await sensor.CollectAsync();
                        await mqttPublisher.PublishAsync(sensor.Metadata.SensorStateTopic, sensorValueFormatter.Format(value), false, cancellationToken: stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Failed to publish sensor {Sensor}", sensor.Metadata.Key);
                    }
                }
                logger.LogDebug("{sensorCount} sensors published", _sensors.Count());
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
