using System.Text;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;
using Win2Mqtt.Common;

namespace Win2Mqtt.Infra
{
    public class MqttPublisher(IMqttClient client, ILogger<MqttPublisher> logger) : IMqttPublisher
    {
        private readonly IMqttClient _client = client;
        private readonly ILogger<MqttPublisher> _logger = logger;

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
    }
}