using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Win2Mqtt.Common;

namespace Win2Mqtt.Options
{
    public class Win2MqttOptions
    {
        public static readonly string SectionName = Constants.AppId;

        /// <summary>
        /// Returns the base topic for this device in the MQTT broker.
        /// It just adds the app prefix (win2mqtt) to the device identifier and adds a slash between them.
        /// e.g.: win2mqtt/winsrv02
        /// </summary>
        public string MqttBaseTopic => $"{Constants.Win2MqttTopic}/{SanitizeHelpers.Sanitize(DeviceIdentifier)}";

        /// <summary>
        /// Returns the unique ID for this device (ie, for use in Home Assistant).
        /// It just adds the app prefix (win2mqtt) to the device identifier and adds an underscore between them.
        /// e.g.: win2mqtt_winsrv02
        /// </summary>
        public string DeviceUniqueId => $"{Constants.Win2MqttTopic}_{SanitizeHelpers.Sanitize(DeviceIdentifier)}";

        [Required]
        public required MqttBrokerOptions Broker { get; set; }

        [Required()]
        [RegularExpression(@"[^/\\#]+$")]
        public string DeviceIdentifier { get; set; } = Environment.MachineName;

        public int MqttTopicQoS { get; set; } = 1;

        public int TimerInterval { get; set; } = 5;

        [Required]
        public required Dictionary<string, SensorOptions> Sensors { get; set; } = [];
        [Required]
        public Dictionary<string, MultiSensorOptions> MultiSensors { get; set; } = [];

        [Required]
        public Dictionary<string, ListenerOptions> Listeners { get; set; } = [];
    }
}
