using System.Collections.Generic;

namespace Win2Mqtt.Options
{
    public class MultiSensorOptions
    {
        public bool Enabled { get; set; } = true;
        public string? Topic { get; set; }

        public required Dictionary<string, SensorOptions> Sensors { get; set; }

    }
}
