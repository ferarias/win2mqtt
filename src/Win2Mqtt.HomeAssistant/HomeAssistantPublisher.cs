using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantPublisher(
        IMqttPublisher connector,
        IOptions<Win2MqttOptions> options,
        ILogger<HomeAssistantPublisher> logger)
        : IHomeAssistantPublisher
    {
        public async Task NotifyOnlineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{options.Value.MqttBaseTopic}/status";
            await connector.PublishAsync(statusTopic, "online", retain: true, cancellationToken);
            logger.LogDebug("Published HA online status in {Topic}", statusTopic);
        }

        public async Task NotifyOfflineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{options.Value.MqttBaseTopic}/status";
            await connector.PublishAsync(statusTopic, "offline", retain: true, cancellationToken: cancellationToken);
            logger.LogDebug("Published HA offline status for {Topic}", statusTopic);
        }

    }
}
