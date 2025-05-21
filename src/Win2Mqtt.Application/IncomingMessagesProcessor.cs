using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public class IncomingMessagesProcessor(
        IEnumerable<IMqttActionHandler> handlers,
        IMqttPublisher connector,
        ISensorValueFormatter sensorValueFormatter,
        IOptions<Win2MqttOptions> options,
        ILogger<IncomingMessagesProcessor> logger) : IIncomingMessagesProcessor
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly Dictionary<string, IMqttActionHandler> _handlers = handlers.ToDictionary(h => h.GetType().Name.Replace("Handler", ""), h => h, StringComparer.OrdinalIgnoreCase);

        public async Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var match = _options
                    .Listeners
                    .FirstOrDefault(kv => kv.Value.Topic.Equals(subtopic, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(match.Key) || !match.Value.Enabled)
                {
                    logger.LogWarning("No enabled listener for subtopic `{Subtopic}`", subtopic);
                    return;
                }

                if (_handlers.TryGetValue(match.Key, out var handler))
                {
                    await handler.HandleAsync(message, cancellationToken);
                }
                else
                {
                    logger.LogWarning("Handler not implemented for listener `{Listener}`", match.Key);
                }

                //TODO: Notifier
                //        var notifierParameters = JsonSerializer.Deserialize<NotificationMessage>(message);
                //        if (notifierParameters != null)
                //        {
                //            Notifier.Show(notifierParameters);
                //        }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on processing message received");
            }
        }
    }
}
