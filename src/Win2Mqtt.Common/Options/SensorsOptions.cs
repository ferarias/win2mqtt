namespace Win2Mqtt.Options
{
    public class SensorOptions
    {
        public required string Topic { get; set; }
        public bool Enabled { get; set; } = true;

    }
}
