using System.ComponentModel.DataAnnotations;

namespace Win2Mqtt.Options
{
    public class MqttBrokerOptions
    {
        [Required]
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
