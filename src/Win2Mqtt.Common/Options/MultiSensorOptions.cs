using System.Collections.Generic;

namespace Win2Mqtt.Options
{
    public class MultiSensorOptions
    {
        public string Topic { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, SensorOptions> Sensors { get; set; }

    }
}
