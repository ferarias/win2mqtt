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

        public int TimerInterval { get; set; } = 5;

        public bool EnableNotifications { get; set; }

        [Required]
        public SensorsOptions Sensors { get; set; }
        [Required]
        public IncomingMessagesOptions IncomingMessages { get; set; }

        [Required]
        public Dictionary<string, ListenerOptions> Listeners { get; set; }
    }
}
