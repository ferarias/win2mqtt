using System.Text;
using System.Text.RegularExpressions;
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
        : IMqttConnectionManager, IMqttPublisher, IMqttSubscriber, IAsyncDisposable
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger _logger = logger;
        private readonly string _mqttBaseTopic = $"{Constants.ServiceBaseTopic}/{options.Value.MachineIdentifier}/";
        private IMqttClient? _client;

        private string PrefixWithBaseTopic(string topic) => $"{_mqttBaseTopic}{topic}";

        private static string Sanitize(string value) => Regex.Replace(value.ToLowerInvariant(), @"[^a-z0-9_]+", "_");

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

                var mqttClientOptions = mqttOptionsBuilder
                    .WithWillTopic(PrefixWithBaseTopic("status"))
                    .WithWillPayload("offline")
                    .WithWillRetain(true)
                    .Build();

                var response = await _client.ConnectAsync(mqttClientOptions, CancellationToken.None);

                _logger.LogInformation("The MQTT client is connected.");

                await PublishAsync($"{_mqttBaseTopic}status", "online", retain: true);

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
                            string topic = PrefixWithBaseTopic(Sanitize(listener.Value.Topic));
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

        public async Task PublishForDeviceAsync(string topic, string message, bool retain = false) =>
            await PublishAsync(PrefixWithBaseTopic(topic), message, retain);

        public async Task PublishAsync(string topic, string message, bool retain = false)
        {
            if (_client?.IsConnected == true)
            {
                var sanitizedTopic = Sanitize(topic);
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(sanitizedTopic)
                    .WithPayload(Encoding.UTF8.GetBytes(message))
                    .WithRetainFlag(retain)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await _client.PublishAsync(mqttMessage);
                _logger.LogDebug("Message published: {Topic} value {Message}", sanitizedTopic, message);
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
