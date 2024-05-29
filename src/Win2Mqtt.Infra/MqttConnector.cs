using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using System.Text;
using Win2Mqtt.Infra.HardwareSensors;
using Win2Mqtt.Options;


namespace Win2Mqtt.Client.Mqtt
{
    public class MqttConnector(IOptions<Win2MqttOptions> options, INotifier toastMessage, ILogger<MqttConnector> logger) : IMqttConnector
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly INotifier _notifier = toastMessage;
        private readonly ILogger _logger = logger;
        private readonly string _mqttTopic = options.Value.MqttTopic;
        private IMqttClient? _client;

        public bool Connected => _client?.IsConnected == true;

        private string GetFullTopic(string topic) => _mqttTopic.Replace("#", topic);


        public async Task<bool> ConnectAsync()
        {
            try
            {
                var mqttFactory = new MqttFactory();
                _client = mqttFactory.CreateMqttClient();
                var mqttOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(_options.Broker.Server, _options.Broker.Port) // MQTT broker address and port
                    .WithCredentials(_options.Broker.Username, _options.Broker.Password) // Set username and password
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithCleanSession()
                .Build();

                var response = await _client.ConnectAsync(mqttOptions, CancellationToken.None);

                _logger.LogInformation("The MQTT client is connected.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Could not connect; check connection settings");
                throw;
            }

            try
            {
                if (Connected)
                {

                    _client.ApplicationMessageReceivedAsync += ClientMqttMsgPublishReceivedAsync;

                    _logger.LogInformation("connected");


                    var r = new List<string>();
                    var qosLevelse = new List<byte[]>();

                    await _client.SubscribeAsync(_mqttTopic + "/monitor/set", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/mute/set", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/volume/set", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/hibernate", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/suspend", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/reboot", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/reboot", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/shutdown", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/tts", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/toast", MqttQualityOfServiceLevel.ExactlyOnce);
                    await _client.SubscribeAsync(_mqttTopic + "/cmd", MqttQualityOfServiceLevel.ExactlyOnce);

                    return true;
                }
            }

            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Could not subscribe; check settings");
                throw;
            }
            return false;
        }

        public async Task DisconnectAsync()
        {
            if (_client?.IsConnected == true)
            {
                await _client.DisconnectAsync(new MqttClientDisconnectOptionsBuilder().WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection).Build());
            }

        }

        public async Task PublishRawAsync(string topic, byte[] bytes)
        {
            if (_client?.IsConnected == true)
            {
                var fullTopic = GetFullTopic(topic);
                await _client.PublishBinaryAsync(fullTopic, bytes);
                _logger.LogInformation("Bytes published: {topic}", fullTopic);
            }
        }
        public async Task PublishMessageAsync(string topic, string message, bool retain = false)
        {
            if (_client?.IsConnected == true)
            {
                var fullTopic = GetFullTopic(topic);
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(fullTopic)
                    .WithPayload(Encoding.UTF8.GetBytes(message))
                    .WithRetainFlag(retain)
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                    .Build();

                await _client.PublishAsync(mqttMessage);
                _logger.LogInformation("message published: {fullTopic} value {message}", fullTopic, message);
            }
        }

        private async Task ClientMqttMsgPublishReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                _logger.LogInformation("Message received in {topic}: `{message}`", e.ApplicationMessage.Topic, message);

                string topLevel = _mqttTopic.Replace("/#", "");
                string subtopic = e.ApplicationMessage.Topic.Replace(topLevel + "/", "");

                await MessageReceivedAsync(subtopic, message);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception receiving");
            }

        }
        private async Task MessageReceivedAsync(string subtopic, string message)
        {
            try
            {
                switch (subtopic)
                {
                    case "app/running":
                        await PublishMessageAsync($"app/running/{message}", Sensors.HardwareSensors.Processes.IsRunning(message));
                        break;
                    case "app/close":
                        await PublishMessageAsync($"app/running/{message}", Sensors.HardwareSensors.Processes.Close(message));
                        break;

                    case "monitor/set":
                        if (message == "1" || message == "on")
                        {
                            Sensors.HardwareSensors.Monitor.TurnOn();
                            await PublishMessageAsync("monitor", "1");
                        }
                        else if (message == "0" || message == "off")
                        {
                            Sensors.HardwareSensors.Monitor.TurnOff();
                            await PublishMessageAsync("monitor", "0");
                        }
                        break;

                    case "hibernate":
                        PowerManagement.HibernateSystem();
                        break;

                    case "suspend":
                        PowerManagement.SuspendSystem();
                        break;

                    case "reboot":
                        PowerManagement.Reboot(message.AsInt(10));
                        break;

                    case "shutdown":
                        PowerManagement.Shutdown(message.AsInt(10));
                        break;

                    case "toast":
                        string[] words = message.Split(',');
                        if (words.Length >= 3)
                        {
                            string imageUrl = words[^1];
                            _notifier.ShowImage(words, imageUrl);
                        }
                        else
                        {
                            _notifier.ShowText(words);
                        }
                        break;

                    case "cmd":
                        var parameters = JsonConvert.DeserializeObject<CommandParameters>(message);
                        if (parameters != null)
                        {
                            Commands.RunCommand(parameters);
                        }

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