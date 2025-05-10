namespace Win2Mqtt.SystemMetrics.Windows
{
    public class TimeSensor : ISensor
    {
        public IDictionary<string, string> Collect() => new Dictionary<string, string> { { "timestamp", DateTime.UtcNow.ToString("o") } };
    }
}
