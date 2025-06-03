using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;

namespace Win2Mqtt.Broker.MQTTNet
{
    public class MqttSubscriber(
        IMqttClient client,
        IOptions<Win2MqttOptions> options,
        ISystemActionFactory actionFactory,
        ISystemActionsHandler incomingMessagesProcessor,
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

                    // Define what happens when a message is received
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

                    // Subscribe to the topics defined in options
                    foreach (var action in actionFactory.GetEnabledActions())
                    {
                        var actionMetadata = action.Value.Metadata;
                        await _client.SubscribeAsync(actionMetadata.CommandTopic, MqttQualityOfServiceLevel.ExactlyOnce, cancellationToken);
                        logger.LogDebug("Subscribed to MQTT topic `{Topic}`.", actionMetadata.CommandTopic);
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