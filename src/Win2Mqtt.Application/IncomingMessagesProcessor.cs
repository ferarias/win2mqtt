using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemMetrics;

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
        private readonly IDictionary<string, IMqttActionHandler> _handlers = handlers.ToDictionary(h => h.GetType().Name.Replace("Handler", ""), h => h, StringComparer.OrdinalIgnoreCase);

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


                //// Find in options the Listener that corresponds to this topic
                //var (listener, options) = _options.Listeners.First(l => l.Value.Topic.Equals(subtopic));

                //var processRunningTopic = $"{_options.MqttBaseTopic}/status/process/running/{message}";
                //switch (listener)
                //{
                //    case "SendMessage":
                //        var notifierParameters = JsonSerializer.Deserialize<NotificationMessage>(message);
                //        if (notifierParameters != null)
                //        {
                //            Notifier.Show(notifierParameters);
                //        }
                //        break;

                //    case "Exec":
                //        var execParameters = JsonSerializer.Deserialize<CommandParameters>(message);
                //        if (execParameters != null)
                //        {
                //            Commands.RunCommand(execParameters);
                //        }

                //        break;
                //    case "ProcessRunning":
                //        await connector.PublishAsync(processRunningTopic, sensorValueFormatter.Format(Processes.IsRunning(message)), false, cancellationToken: cancellationToken);
                //        break;

                //    case "ProcessClose":
                //        await connector.PublishAsync(processRunningTopic, sensorValueFormatter.Format(Processes.Close(message)), false, cancellationToken: cancellationToken);
                //        break;

                //    default:
                //        _logger.LogError("An invalid topic or message received: `{topic}` `{message}`", subtopic, message);
                //        break;

                //}
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on processing message received");
            }
        }
    }
}
