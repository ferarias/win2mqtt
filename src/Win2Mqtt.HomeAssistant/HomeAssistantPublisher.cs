using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Common;
using Win2Mqtt.Options;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantPublisher(
        ISensorFactory sensorFactory,
        IMqttPublisher connector,
        IOptions<Win2MqttOptions> options,
        ILogger<HomeAssistantPublisher> logger)
        : IHomeAssistantPublisher
    {
        private readonly IEnumerable<ISensorWrapper> _sensors = sensorFactory.GetEnabledSensors();

        public async Task PublishSensorsDiscoveryAsync(CancellationToken cancellationToken = default)
        {
            var machineIdentifier = SanitizeHelpers.Sanitize(options.Value.MachineIdentifier);

            foreach (var sensor in _sensors)
            {
                (string homeAssistantDiscoveryTopic, string payload) = 
                    HomeAssistantDiscoveryHelper.GetSensorDiscoveryPayload(machineIdentifier, sensor.Metadata);

                await connector.PublishAsync(homeAssistantDiscoveryTopic, payload, retain: true, cancellationToken: cancellationToken);
                logger.LogInformation("Published HA binary_sensor config for {sensor}", sensor.Metadata.Key);

            }
        }

        public async Task NotifyOnlineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{Constants.Win2MqttTopic}/{options.Value.MachineIdentifier}/status";
            await connector.PublishAsync(statusTopic, "online", retain: true, cancellationToken);
            logger.LogDebug("Published HA online status for {machineIdentifier}", options.Value.MachineIdentifier);
        }

        public async Task NotifyOfflineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{Constants.Win2MqttTopic}/{options.Value.MachineIdentifier}/status";
            await connector.PublishAsync(statusTopic, "offline", retain: true, cancellationToken: cancellationToken);
            logger.LogDebug("Published HA offline status for {machineIdentifier}", options.Value.MachineIdentifier);
        }

    }
}
