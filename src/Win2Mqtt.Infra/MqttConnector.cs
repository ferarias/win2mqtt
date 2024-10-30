using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Win2Mqtt.Options;

namespace Win2Mqtt.Infra
{
    public class MqttConnector(
        IOptions<Win2MqttOptions> options,
        ILogger<MqttConnector> logger)
        : IMqttConnector, IAsyncDisposable
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger _logger = logger;
        private readonly string _mqttBaseTopic = $"win2mqtt/{options.Value.MqttTopic}/";
        private IMqttClient? _client;

        private string GetFullTopic(string topic) => $"{_mqttBaseTopic}{topic}";

        public async Task<bool> ConnectAsync()
        {
            _logger.LogInformation("Connecting to MQTT broker.");
            try
            {
                var mqttFactory = new MqttFactory();
                _client = mqttFactory.CreateMqttClient();
                var mqttOptionsBuilder = new MqttClientOptionsBuilder()
                    .WithTcpServer(_options.Broker.Server, _options.Broker.Port)
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithCleanSession();
                if (!string.IsNullOrWhiteSpace(_options.Broker.Username) || !string.IsNullOrWhiteSpace(_options.Broker.Password))
                {
                    mqttOptionsBuilder.WithCredentials(_options.Broker.Username, _options.Broker.Password);
                }

                var response = await _client.ConnectAsync(mqttOptionsBuilder.Build(), CancellationToken.None);

                _logger.LogInformation("The MQTT client is connected.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Could not connect; check connection settings");
            }
            return false;
        }
        public async Task<bool> SubscribeAsync(Func<string, string, Task> processMessageAsync)
        {
            _logger.LogInformation("Subscribing to topics.");
            try
            {
                if (_client?.IsConnected == true)
                {
                    _client.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs e) =>
                    {
                        _logger.LogInformation("New message received in `{topic}`.", e.ApplicationMessage.Topic);
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
                            string topic = GetFullTopic(listener.Value.Topic);
                            await _client.SubscribeAsync(topic, MqttQualityOfServiceLevel.ExactlyOnce);
                            _logger.LogInformation("Subscribed to MQTT subtopic `{subtopic}`.", topic);
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

        public async Task DisconnectAsync()
        {
            if (_client?.IsConnected == true)
            {
                var unsubscribeOptions = new MqttClientUnsubscribeOptionsBuilder()
                    .WithTopicFilter($"{_mqttBaseTopic}#")
                    .Build();
                await _client.UnsubscribeAsync(unsubscribeOptions);
                _logger.LogInformation("Unsubscribed from MQTT topics.");

                var disconnectOptions = new MqttClientDisconnectOptionsBuilder()
                    .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                    .Build();
                await _client.DisconnectAsync(disconnectOptions);
                _logger.LogInformation("The MQTT client is disconnected.");
            }
        }

        public async Task PublishRawAsync(string subtopic, byte[] bytes)
        {
            if (_client?.IsConnected == true)
            {
                var topic = GetFullTopic(subtopic);

                await _client.PublishBinaryAsync(topic, bytes);
                _logger.LogDebug("Bytes published: {subtopic}", topic);
            }
        }

        public async Task PublishMessageAsync(string subtopic, string message, bool retain = false)
        {
            if (_client?.IsConnected == true)
            {
                var topic = GetFullTopic(subtopic);
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(message))
                    .WithRetainFlag(retain)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await _client.PublishAsync(mqttMessage);
                _logger.LogDebug("Message published: {subtopic} value {message}", topic, message);
            }
        }


        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            await DisconnectAsync();
        }


    }
}