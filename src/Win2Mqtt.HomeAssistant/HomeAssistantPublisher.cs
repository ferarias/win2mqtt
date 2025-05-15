using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantPublisher(
        IMqttPublisher mqttPublisher,
        ISensorValueFormatter sensorValueFormatter,
        IOptions<Win2MqttOptions> options,
        ILogger<HomeAssistantPublisher> logger)
        : IMessagePublisher
    {
        private readonly object DeviceInfo = HomeAssistantDiscoveryHelper.GetHADeviceInfo(options.Value.DeviceUniqueId);

        public async Task NotifyOnlineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{options.Value.MqttBaseTopic}/status";
            await mqttPublisher.PublishAsync(statusTopic, "online", retain: true, cancellationToken);
            logger.LogDebug("Published HA online status in {Topic}", statusTopic);
        }

        public async Task NotifyOfflineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{options.Value.MqttBaseTopic}/status";
            await mqttPublisher.PublishAsync(statusTopic, "offline", retain: true, cancellationToken: cancellationToken);
            logger.LogDebug("Published HA offline status for {Topic}", statusTopic);
        }

        public async Task PublishSensorValue(ISensorWrapper sensor, object? value, CancellationToken cancellationToken = default)
        {
            if (sensor.Metadata.SensorStateTopic == null)
            {
                logger.LogWarning("Sensor {Sensor} has no state topic", sensor.Metadata.Key);
                return;
            }
            try
            {
                await mqttPublisher.PublishAsync(sensor.Metadata.SensorStateTopic, sensorValueFormatter.Format(value), false, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to publish sensor {Sensor}", sensor.Metadata.Key);
            }
        }

        public async Task PublishSensorDiscoveryMessage(ISensorWrapper sensor, CancellationToken cancellationToken = default)
        {
            await mqttPublisher.PublishAsync(
                    HomeAssistantDiscoveryHelper.GetHADiscoveryMessage(options.Value.MqttBaseTopic, sensor.Metadata, DeviceInfo),
                    retain: true,
                    cancellationToken: cancellationToken);

            logger.LogInformation("Published HA binary_sensor config for {sensor}", sensor.Metadata.Key);

        }
    }
}
