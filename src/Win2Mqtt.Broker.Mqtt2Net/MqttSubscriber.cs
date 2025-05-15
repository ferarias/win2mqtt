using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using Win2Mqtt.Common;
using Win2Mqtt.Options;

namespace Win2Mqtt.Broker.MQTTNet
{
    public class MqttSubscriber(IMqttClient client, IOptions<Win2MqttOptions> options, ILogger<MqttSubscriber> logger) : IMqttSubscriber
    {
        private readonly IMqttClient _client = client;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<MqttSubscriber> _logger = logger;

        private readonly string _mqttBaseTopic = $"{Constants.Win2MqttTopic}/{options.Value.MachineIdentifier}/";

        public async Task<bool> SubscribeAsync(
            Func<string, string, CancellationToken, Task> ProcessIncomingMessageAsync,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Subscribing to topics.");
            try
            {
                if (_client?.IsConnected != true) return false;

                _client.ApplicationMessageReceivedAsync += async (e) =>
                {
                    _logger.LogDebug("New message received in `{Topic}`.", e.ApplicationMessage.Topic);
                    try
                    {
                        var operation = e.ApplicationMessage.Topic.Replace(_mqttBaseTopic, "");
                        var message = e.ApplicationMessage.ConvertPayloadToString();

                        await ProcessIncomingMessageAsync(operation, message, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception receiving");
                    }
                };

                foreach (var listener in _options.Listeners)
                {
                    if (listener.Value.Enabled)
                    {
                        var sanitizedTopic = SanitizeHelpers.Sanitize(listener.Value.Topic);
                        string topic = $"{_mqttBaseTopic}{sanitizedTopic}";
                        await _client.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce, cancellationToken);
                        _logger.LogDebug("Subscribed to MQTT topic `{Topic}`.", topic);
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Could not subscribe; check settings");
            }
            return false;
        }
    }
}