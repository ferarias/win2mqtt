using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Win2Mqtt.Options;

namespace Win2Mqtt.Infra
{
    public class MqttSubscriber(IMqttClient client, IOptions<Win2MqttOptions> options, ILogger<MqttSubscriber> logger) : IMqttSubscriber
    {
        private readonly IMqttClient _client = client;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<MqttSubscriber> _logger = logger;

        private readonly string _mqttBaseTopic = $"{Constants.ServiceBaseTopic}/{options.Value.MachineIdentifier}/";

        public async Task<bool> SubscribeAsync(Func<string, string, Task> processMessageAsync)
        {
            _logger.LogInformation("Subscribing to topics.");
            try
            {
                if (_client?.IsConnected == true)
                {
                    _client.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs e) =>
                    {
                        _logger.LogInformation("New message received in `{sanitizedTopic}`.", e.ApplicationMessage.Topic);
                        try
                        {
                            var operation = e.ApplicationMessage.Topic.Replace(_mqttBaseTopic, "");
                            var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

                            await processMessageAsync(operation, message);
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
                            await _client.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce);
                            _logger.LogInformation("Subscribed to MQTT topic `{topic}`.", topic);
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Could not subscribe; check settings");
            }
            return false;
        }
    }
}