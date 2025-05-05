using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using Win2Mqtt.Options;

namespace Win2Mqtt.Infra
{
    public class MqttPublisher(IMqttClient client, IOptions<Win2MqttOptions> options, ILogger<MqttPublisher> logger) : IMqttPublisher
    {
        private readonly IMqttClient _client = client;
        private readonly ILogger<MqttPublisher> _logger = logger;

        private readonly string _mqttBaseTopic = $"{Constants.ServiceBaseTopic}/{options.Value.MachineIdentifier}/";

        public async Task PublishAsync(string topic, string message, bool retain = false, CancellationToken cancellationToken = default)
        {
            if (_client.IsConnected)
            {
                var sanitizedTopic = SanitizeHelpers.Sanitize(topic);
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(sanitizedTopic)
                    .WithPayload(Encoding.UTF8.GetBytes(message))
                    .WithRetainFlag(retain)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

                await _client.PublishAsync(mqttMessage, cancellationToken);
                _logger.LogDebug("Message published: {Topic} value {Message}", sanitizedTopic, message);
            }
        }

        public async Task PublishForDeviceAsync(string topic, string message, bool retain = false, CancellationToken cancellationToken = default) =>
                    await PublishAsync($"{_mqttBaseTopic}{topic}", message, retain, cancellationToken);
    }
}