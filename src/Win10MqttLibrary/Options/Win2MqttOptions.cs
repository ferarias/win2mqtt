using System.ComponentModel.DataAnnotations;

namespace Win2Mqtt.Options
{
    public class Win2MqttOptions
    {
        [Required]
        public MqttBrokerOptions Broker { get; set; }
        public string MqttTopic { get; set; } = "win2mqtt";
        public int TimerInterval { get; set; } = 5;
        public bool EnableNotifications { get; set; }
        [Required]
        public SensorsOptions Sensors { get; set; }
    }
    public class MqttBrokerOptions
    {
        [Required]
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SensorsOptions
    {
        public bool CpuSensor { get; set; }
        public bool FreeMemorySensor { get; set; }
        public bool IsComputerUsed { get; set; }
        public bool BatterySensor { get; set; }
        public bool DiskSensor { get; set; }
        public bool NetworkSensor { get; set; }

        public bool Hibernate { get; set; }
        public bool Shutdown { get; set; }
        public bool Reboot { get; set; }
        public bool Suspend { get; set; }

        public bool Monitor { get; set; }
        public bool App { get; set; }
    }

}
