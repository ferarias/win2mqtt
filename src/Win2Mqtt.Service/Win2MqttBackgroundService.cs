using Microsoft.Extensions.Options;
using Win2Mqtt.Common;
using Win2Mqtt.Common.Options;
using Win2Mqtt.HomeAssistant;

namespace Win2Mqtt.Service
{
    public class Win2MqttBackgroundService(
        IMqttConnectionManager mqttConnectionManager,
        IMqttSubscriber mqttSubscriber,
        IMqttPublisher mqttPublisher,
        ISensorDataCollector sensorDataCollector,
        IIncomingMessagesProcessor incomingMessagesProcessor,
        IHomeAssistantDiscoveryPublisher haDiscoveryPublisher,
        IOptions<Win2MqttOptions> options,
        ILogger<Win2MqttBackgroundService> logger) : BackgroundService
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker started");
            try
            {
                // Connect to MQTT broker
                await ConnectToMqttBrokerAsync(stoppingToken);

                // Subscribe to incoming messages and  process them with IIncomingMessagesProcessor.ProcessMessageAsync()
                await SubscribeToIncomingMessagesAsync(stoppingToken);

                // Publish Home Assistant discovery messages
                await haDiscoveryPublisher.PublishSensorsDiscoveryAsync(stoppingToken);

                var statusTopic = $"{Constants.ServiceBaseTopic}/{_options.MachineIdentifier}/status";
                await mqttPublisher.PublishAsync(statusTopic, "online", retain: true, stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    // Allow only one thread collecting system information
                    await _semaphore.WaitAsync(stoppingToken);
                    try
                    {
                        // Collect system information
                        var sensorsData = await sensorDataCollector.CollectSystemDataAsync();

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

            }
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
                logger.LogInformation("Cancelling pending operations.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }

        private async Task ConnectToMqttBrokerAsync(CancellationToken stoppingToken)
        {
            while (!(await mqttConnectionManager.ConnectAsync(stoppingToken)))
            {
                logger.LogWarning("MQTT connection failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        private async Task SubscribeToIncomingMessagesAsync(CancellationToken stoppingToken)
        {
            while (!(await mqttSubscriber.SubscribeAsync(incomingMessagesProcessor.ProcessMessageAsync, stoppingToken)))
            {
                logger.LogWarning("MQTT subscription failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            var statusTopic = $"{Constants.ServiceBaseTopic}/{_options.MachineIdentifier}/status";
            await mqttPublisher.PublishAsync(statusTopic, "offline", retain: true, cancellationToken: cancellationToken);
            await mqttConnectionManager.DisconnectAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
