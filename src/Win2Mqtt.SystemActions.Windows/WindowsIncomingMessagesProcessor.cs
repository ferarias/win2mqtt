using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemActions.Windows
{
    public class WindowsIncomingMessagesProcessor(IMqttPublisher connector,
        IOptions<Win2MqttOptions> options,
        ILogger<WindowsIncomingMessagesProcessor> logger) : IIncomingMessagesProcessor
    {
        private readonly IMqttPublisher _connector = connector;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<WindowsIncomingMessagesProcessor> _logger = logger;

        public async Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                // Find in options the Listener that corresponds to this topic
                var (listener, options) = _options.Listeners.First(l => l.Value.Topic.Equals(subtopic));

                var processRunningTopic = $"{_options.MqttBaseTopic}/status/process/running/{message}";
                switch (listener)
                {
                    case "SendMessage":
                        var notifierParameters = JsonSerializer.Deserialize<NotificationMessage>(message);
                        if (notifierParameters != null)
                        {
                            Notifier.Show(notifierParameters);
                        }
                        break;

                    case "Exec":
                        var execParameters = JsonSerializer.Deserialize<CommandParameters>(message);
                        if (execParameters != null)
                        {
                            Commands.RunCommand(execParameters);
                        }

                        break;
                    case "ProcessRunning":
                        await _connector.PublishAsync(processRunningTopic, Processes.IsRunning(message).BooleanToMqttOneOrZero(), false, cancellationToken: cancellationToken);
                        break;

                    case "ProcessClose":
                        await _connector.PublishAsync(processRunningTopic, Processes.Close(message).BooleanToMqttOneOrZero(), false, cancellationToken: cancellationToken);
                        break;

                    case "Hibernate":
                        PowerManagement.HibernateSystem();
                        break;

                    case "Suspend":
                        PowerManagement.SuspendSystem();
                        break;

                    case "Reboot":
                        if (int.TryParse(message, out int rebootDelay))
                            PowerManagement.Reboot(message.MqttAsInt(rebootDelay));
                        else
                            PowerManagement.Reboot(message.MqttAsInt(10));
                        break;

                    case "Shutdown":
                        if (int.TryParse(message, out int shutdownDelay))
                            PowerManagement.Reboot(message.MqttAsInt(shutdownDelay));
                        else
                            PowerManagement.Reboot(message.MqttAsInt(10));
                        break;

                    default:
                        _logger.LogError("An invalid topic or message received: `{topic}` `{message}`", subtopic, message);
                        break;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception on processing message received");
            }
        }
    }
}
