using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.Service
{
    public sealed class Win2MqttService(
        IMqttConnectionManager connectionManager,
        ISensorFactory sensorFactory,
        IMessagePublisher messagePublisher,
        IMessageListener messageListener,
        IOptionsMonitor<Win2MqttOptions> options,
        ILogger<Win2MqttService> logger)
    {
        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        private readonly IEnumerable<ISensorWrapper> _activeSensors = sensorFactory.GetEnabledSensors();


        public async Task StartAsync(CancellationToken stoppingToken)
        {
            // Connect to MQTT broker
            await connectionManager.ConnectAsync(stoppingToken);

            // Subscribe to incoming messages
            await messageListener.SubscribeToIncomingMessagesAsync(stoppingToken);

            // Publish Home Assistant discovery messages
            foreach (var sensor in _activeSensors)
            {
                await messagePublisher.PublishSensorDiscoveryMessage(sensor.Metadata, stoppingToken);
            }

            // Publish online status
            await messagePublisher.PublishOnlineStatus(stoppingToken);
        }

        public async Task CollectAndPublish(CancellationToken stoppingToken)
        {
            // Allow only one thread collecting system information
            await _semaphore.WaitAsync(stoppingToken);
            try
            {
                foreach (var sensor in _activeSensors)
                {
                    var sensorValue = await sensor.CollectAsync();
                    await messagePublisher.PublishSensorValue(sensor, sensorValue, stoppingToken);
                }
                logger.LogDebug("{sensorCount} sensors published", _activeSensors.Count());
            }
            finally
            {
                _semaphore.Release();
            }
            await Task.Delay(TimeSpan.FromSeconds(options.CurrentValue.TimerInterval), stoppingToken);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Disonnect from MQTT broker
            await messagePublisher.PublishOfflineStatus(cancellationToken);

            // Publish offline status
            await connectionManager.DisconnectAsync(cancellationToken);
        }
    }

}
