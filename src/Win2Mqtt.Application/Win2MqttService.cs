using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.Application
{
    public sealed class Win2MqttService(
        IMqttConnectionManager connectionManager,
        ISensorFactory sensorFactory,
        IMessagePublisher publisher,
        IMqttSubscriber subscriber)
    {

        private readonly IEnumerable<ISensorWrapper> _activeSensors = sensorFactory.GetEnabledSensors();

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            // Connect to MQTT broker
            await connectionManager.ConnectAsync(stoppingToken);

            // Subscribe to incoming messages
            await subscriber.SubscribeAsync(stoppingToken);

            // Publish Home Assistant discovery messages
            foreach (var sensor in _activeSensors)
            {
                await publisher.PublishSensorDiscoveryMessage(sensor.Metadata, stoppingToken);
            }

            // Publish online status
            await publisher.PublishOnlineStatus(stoppingToken);
        }

        public async Task CollectAndPublish(CancellationToken stoppingToken)
        {
            foreach (var sensor in _activeSensors)
            {
                var sensorValue = await sensor.CollectAsync();
                await publisher.PublishSensorValue(sensor, sensorValue, stoppingToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Disonnect from MQTT broker
            await publisher.PublishOfflineStatus(cancellationToken);

            // Publish offline status
            await connectionManager.DisconnectAsync(cancellationToken);
        }
    }

}
