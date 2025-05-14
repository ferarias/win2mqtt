namespace Win2Mqtt.Options
{
    public class ListenerOptions
    {
        public required string Topic { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
