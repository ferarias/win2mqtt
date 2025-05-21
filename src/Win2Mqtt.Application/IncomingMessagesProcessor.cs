using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public class IncomingMessagesProcessor : IIncomingMessagesProcessor
    {
        private readonly Dictionary<string, GenericMqttActionHandlerWrapper> _handlers = new(StringComparer.OrdinalIgnoreCase);
        private readonly Win2MqttOptions _options;
        private readonly ILogger<IncomingMessagesProcessor> _logger;

        public IncomingMessagesProcessor(
            IEnumerable<IMqttActionHandlerMarker> handlers,
            IMqttPublisher connector,
            ISensorValueFormatter sensorValueFormatter,
            IOptions<Win2MqttOptions> options,
            ILogger<IncomingMessagesProcessor> logger)
        {
            _logger = logger;
            _options = options.Value;

            _handlers.EnsureCapacity(handlers.Count());
            foreach (var handler in handlers)
            {
                if (handler is null) continue;

                string topic = handler.GetType().Name.Replace("Handler", "").ToLower();
                var publishTopic = $"{_options.MqttBaseTopic}/{topic}/result";
                _handlers[topic] = new GenericMqttActionHandlerWrapper(handler, connector, sensorValueFormatter, publishTopic);
            }
        }

        public async Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var match = _options
                    .Listeners
                    .FirstOrDefault(kv => kv.Value.Topic.Equals(subtopic, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(match.Key) || !match.Value.Enabled)
                {
                    _logger.LogWarning("No enabled listener for subtopic `{Subtopic}`", subtopic);
                    return;
                }

                if (_handlers.TryGetValue(match.Key, out var handler))
                {
                    await handler.HandleAsync(message, cancellationToken);
                }
                else
                {
                    _logger.LogWarning("Handler not implemented for listener `{Listener}`", match.Key);
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
                _logger.LogError(ex, "Exception on processing message received");
            }
        }
    }
}
