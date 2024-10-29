using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using System.Text;
using Win2Mqtt.Infra.SystemOperations;
using Win2Mqtt.Options;


namespace Win2Mqtt.Client.Mqtt
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
        public async Task<bool> SubscribeAsync()
        {
            _logger.LogInformation("Subscribing to topics.");
            try
            {
                if (_client?.IsConnected == true)
                {
                    _client.ApplicationMessageReceivedAsync += BrokerOnMessageReceivedAsync;

                    foreach (var listener in _options.Listeners)
                    {
                        if(listener.Value.Enabled)
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

        private async Task BrokerOnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            _logger.LogInformation("New message received in `{topic}`.", e.ApplicationMessage.Topic);
            try
            {
                var operation = e.ApplicationMessage.Topic.Replace(_mqttBaseTopic, "");
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

                await ProcessMessageAsync(operation, message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception receiving");
            }

        }
        private async Task ProcessMessageAsync(string subtopic, string message)
        {
            try
            {
                // Find in options the Listener that corresponds to this topic
                var (listenerKey, lv) = _options.Listeners.First(l => l.Value.Topic.Equals(subtopic));

                switch (listenerKey)
                {
                    case "Hibernate":
                        PowerManagement.HibernateSystem();
                        break;

                    case "Suspend":
                        PowerManagement.SuspendSystem();
                        break;

                    case "Reboot":
                        PowerManagement.Reboot(message.AsInt(10));
                        break;

                    case "Shutdown":
                        PowerManagement.Shutdown(message.AsInt(10));
                        break;

                    case "Monitor":
                        var setMonitorToOn = message.FromMqttAnyBoolean();
                        if (setMonitorToOn.HasValue)
                        {
                            Infra.SystemOperations.Monitor.Set(setMonitorToOn.Value);
                            await PublishMessageAsync("monitor", setMonitorToOn.Value.ToMqttOnOff());
                        }
                        break;

                    case "SendMessage":
                        var notifierParameters = JsonConvert.DeserializeObject<NotifierParameters>(message);
                        if (notifierParameters != null)
                        {
                            Notifier.Show(notifierParameters);
                        }
                        break;

                    case "ProcessRunning":
                        await PublishMessageAsync($"process/running/{message}", Processes.IsRunning(message).ToMqttBoolean());
                        break;

                    case "ProcessClose":
                        await PublishMessageAsync($"process/running/{message}", Processes.Close(message).ToMqttBoolean());
                        break;

                    case "Exec":
                        var execParameters = JsonConvert.DeserializeObject<CommandParameters>(message);
                        if (execParameters != null)
                        {
                            Commands.RunCommand(execParameters);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception on processing message received");
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