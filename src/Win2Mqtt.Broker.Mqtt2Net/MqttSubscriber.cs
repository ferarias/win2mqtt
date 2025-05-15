using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using Win2Mqtt.Common;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;

namespace Win2Mqtt.Broker.MQTTNet
{
    public class MqttSubscriber(
        IMqttClient client,
        IOptions<Win2MqttOptions> options,
        IIncomingMessagesProcessor incomingMessagesProcessor,
        ILogger<MqttSubscriber> logger) : IMqttSubscriber
    {
        private readonly IMqttClient _client = client;
        private readonly Win2MqttOptions _options = options.Value;

        public async Task<bool> SubscribeAsync(CancellationToken cancellationToken = default)
        {
            do
            {
                logger.LogInformation("Subscribing to topics.");
                try
                {
                    if (_client?.IsConnected != true) return false;

                    _client.ApplicationMessageReceivedAsync += async (e) =>
                    {
                        logger.LogDebug("New message received in `{Topic}`.", e.ApplicationMessage.Topic);
                        try
                        {
                            var operation = e.ApplicationMessage.Topic[(options.Value.MqttBaseTopic.Length + 1)..];

                            var message = e.ApplicationMessage.ConvertPayloadToString();
                            await incomingMessagesProcessor.ProcessMessageAsync(operation, message, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Exception receiving");
                        }
                    };

                    foreach (var listener in _options.Listeners)
                    {
                        if (listener.Value.Enabled)
                        {
                            var sanitizedTopic = SanitizeHelpers.Sanitize(listener.Value.Topic);
                            string topic = $"{options.Value.MqttBaseTopic}/{sanitizedTopic}";
                            await _client.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce, cancellationToken);
                            logger.LogDebug("Subscribed to MQTT topic `{Topic}`.", topic);
                        }
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Could not subscribe; check settings");
                }
                logger.LogWarning("MQTT subscription failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

            } while (!cancellationToken.IsCancellationRequested);

            return false;
        }
    }
}