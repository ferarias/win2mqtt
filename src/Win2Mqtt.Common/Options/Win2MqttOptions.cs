using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Win2Mqtt.Options
{
    public class Win2MqttOptions
    {
        public static readonly string SectionName = Constants.AppId;

        [Required]
        public MqttBrokerOptions Broker { get; set; }

        [Required()]
        [RegularExpression(@"[^/\\#]+$")]
        public string MachineIdentifier { get; set; } = Environment.MachineName;

        public int MqttTopicQoS { get; set; } = 1;

        public int TimerInterval { get; set; } = 5;

        [Required]
        public Dictionary<string, SensorOptions> Sensors { get; set; }
        [Required]
        public Dictionary<string, MultiSensorOptions> MultiSensors { get; set; }

        [Required]
        public Dictionary<string, ListenerOptions> Listeners { get; set; }
    }
}
