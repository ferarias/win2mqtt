using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace Win2Mqtt.Client.Mqtt
{
    public class Mqtt : IMqtt
    {
        private MqttClient _client;

        private readonly IToastMessage _toastMessage;
        private readonly ILogger _logger;

        public string GMqtttopic { get; set; }

        public bool IsConnected => _client != null && _client.IsConnected;

        public Mqtt(IToastMessage toastMessage, ILogger logger)
        {
            _toastMessage = toastMessage;
            _logger = logger;
        }
        public void PublishBytes(string topic, byte[] bytes)
        {
            if (_client.IsConnected)
            {
                var fullTopic = FullTopic(topic);
                _client.Publish(fullTopic, bytes);
                _logger.Information("Bytes published: {topic}", fullTopic);
            }
        }
        public void Publish(string topic, string message, bool retain = false)
        {
            var fullTopic = FullTopic(topic);
            if (_client.IsConnected)
            {
                if (retain)
                {
                    _client.Publish(fullTopic, Encoding.UTF8.GetBytes(message), 0, retain);
                }
                else
                {
                    _client.Publish(fullTopic, Encoding.UTF8.GetBytes(message));
                }
                _logger.Information("message published: {fullTopic} value {message}", fullTopic, message);
            }
        }
        public bool Connect(string hostname, int portNumber, string username, string password)
        {
            try
            {
                if (!hostname.IsEmptyOrWhitespaced())
                {
                    try
                    {

                        _client = new MqttClient(hostname, portNumber, false, null, null, MqttSslProtocols.None, null);

                        if (username.IsEmptyOrWhitespaced())
                        {
                            byte code = _client.Connect(Guid.NewGuid().ToString());
                        }
                        else
                        {
                            byte code = _client.Connect(Guid.NewGuid().ToString(), username, password);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"not connected, check connection settings error: {ex.Message}");
                    }

                    try
                    {

                        if (_client.IsConnected)
                        {

                            _client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;
                            _client.MqttMsgSubscribed += ClientMqttMsgSubscribed;
                            _client.MqttMsgPublished += ClientMqttMsgPublished;
                            _client.ConnectionClosed += ClientMqttConnectionClosed;

                            _logger.Information("connected");


                            GMqtttopic = Properties.Settings.Default["mqtttopic"].ToString();

                            var r = new List<string>();
                            var qosLevelse = new List<byte[]>();

                            //r.Add(GMqtttopic + "/#");


                            //r.Add(GMqtttopic + "/app/running");
                            ////qosLevelse.Add(new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                            //r.Add(GMqtttopic + "/monitor/set");
                            //r.Add(GMqtttopic + "/mute/set");
                            //r.Add(GMqtttopic + "/volume/set");
                            //r.Add(GMqtttopic + "/hibernate");
                            //r.Add(GMqtttopic + "/suspend");
                            //r.Add(GMqtttopic + "/reboot");
                            //r.Add(GMqtttopic + "/shutdown");
                            //r.Add(GMqtttopic + "/tts");
                            //r.Add(GMqtttopic + "/toast");
                            //r.Add(GMqtttopic + "/cmd");

                            _client.Subscribe([GMqtttopic + "/monitor/set"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/mute/set"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/volume/set"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/hibernate"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/suspend"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/reboot"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/reboot"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/shutdown"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/tts"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/toast"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);
                            _client.Subscribe([GMqtttopic + "/cmd"], [MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE]);

                            return true;
                        }
                    }

                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    //throw new Exception("not connected, check settings mqttservername not entered");
                }
            }

            catch (Exception ex)
            {
                throw new Exception($"not connected,check settings. Error: {ex.InnerException}");
            }
            return false;
        }

        public string FullTopic(string topic)
        {
            return GMqtttopic.Replace("#", topic);
        }

        public void Disconnect()
        {
            if (_client != null)
            {
                if (IsConnected)
                {
                    _client.Disconnect();
                }
            }
        }

        private void ClientMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            try
            {
                _logger.Information("MessageId = {messageId}; Published = {published}", e.MessageId, e.IsPublished);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception publishing");
            }

        }
        private void ClientMqttConnectionClosed(object sender, System.EventArgs e)
        {
            try
            {
                _logger.Information("Mqtt Connection closed");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception closing connection");
            }

        }
        private void ClientMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            try
            {
                _logger.Information("Subscribed for id = {messageId}", e.MessageId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception subscribing");
            }

        }
        private void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                string message = Encoding.UTF8.GetString(e.Message);
                _logger.Information("Message received in {topic}: `{message}`", e.Topic, message);

                string TopLevel = GMqtttopic.Replace("/#", "");
                string subtopic = e.Topic.Replace(TopLevel + "/", "");

                MessageReceived(subtopic, message);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception receiving");
            }

        }
        private void MessageReceived(string subtopic, string message)
        {
            try
            {
                switch (subtopic)
                {
                    case "app/running":

                        var isRunning = JsonConvert.DeserializeObject<Models.IsRunning>(message);

                        switch (isRunning?.Action)
                        {

                            case "1":
                                Publish($"app/running/{isRunning.ApplicationName}", Sensors.HardwareSensors.Process.IsRunning(message, ""));
                                break;
                            case "0":
                                //close the app
                                Sensors.HardwareSensors.Process.Close(isRunning.ApplicationName);
                                Publish($"app/running/{isRunning.ApplicationName}", "0");
                                break;
                            default:
                                Publish($"app/running/{isRunning.ApplicationName}", Sensors.HardwareSensors.Process.IsRunning(message, ""));
                                break;

                        }
                        break;

                    case "app/close":
                        Publish($"app/running/{message}", Sensors.HardwareSensors.Process.Close(message));
                        break;

                    case "monitor/set":
                        if (message == "1" || message == "on")
                        {
                            Sensors.HardwareSensors.Monitor.TurnOn();
                            Publish("monitor", "1");
                        }
                        else if (message == "0" || message == "off")
                        {
                            Sensors.HardwareSensors.Monitor.TurnOff();
                            Publish("monitor", "0");
                        }
                        break;

                    case "mute/set":
                        break;

                    case "volume/set":
                        break;

                    case "hibernate":
                        Application.SetSuspendState(PowerState.Hibernate, true, true);
                        break;

                    case "suspend":
                        Application.SetSuspendState(PowerState.Suspend, true, true);
                        break;

                    case "reboot":
                        System.Diagnostics.Process.Start("shutdown.exe", $"-r -t {GetDelay(message)}");
                        break;

                    case "shutdown":
                        System.Diagnostics.Process.Start("shutdown.exe", $"-s -t {GetDelay(message)}");
                        break;

                    case "tts":
                        //TODO
                        //SpeechSynthesizer synthesizer = new SpeechSynthesizer
                        //{
                        //    Volume = 100
                        //};
                        //synthesizer.SpeakAsync(message);
                        break;

                    case "toast":
                        string[] words = message.Split(',');
                        if (words.Length >= 3)
                        {
                            string imageUrl = words[words.Length - 1];
                            _toastMessage.ShowImage(words, imageUrl);
                        }
                        else
                        {
                            _toastMessage.ShowText(words);
                        }
                        break;

                    case "cmd":

                        ProcessWindowStyle processWindowStyle = new ProcessWindowStyle();

                        var commandParameters = JsonConvert.DeserializeObject<Models.CommandParameters>(message);

                        switch (Convert.ToInt16(commandParameters.WindowStyle))
                        {
                            case 0:
                                processWindowStyle = ProcessWindowStyle.Normal;
                                break;
                            case 1:
                                processWindowStyle = ProcessWindowStyle.Hidden;
                                break;
                            case 2:
                                processWindowStyle = ProcessWindowStyle.Minimized;
                                break;
                            case 3:
                                processWindowStyle = ProcessWindowStyle.Maximized;
                                break;
                            default:
                                processWindowStyle = ProcessWindowStyle.Normal;
                                break;
                        }


                        ProcessStartInfo startInfo = new ProcessStartInfo(commandParameters.CommandString, commandParameters.ExecParameters)
                        {
                            WindowStyle = processWindowStyle

                        };

                        System.Diagnostics.Process.Start(startInfo);

                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception on processing message received");
            }
        }
        private static int GetDelay(string message)
        {
            var result = int.TryParse(message, out var delay);
            if (result)
            {
                return delay;
            }
            else
            {
                return 10;
            }
        }
        public void PublishDiscovery(string topic, SensorType sensorType)
        {
            switch (sensorType)
            {
                case SensorType.BinarySensor:
                    break;
                case SensorType.Light:
                    break;
                case SensorType.Sensor:
                    break;
                case SensorType.Switch:
                    break;
            }


            //public enum SensorType { Binary_sensor, Switch, Light, Sensor };


        }
    }
}