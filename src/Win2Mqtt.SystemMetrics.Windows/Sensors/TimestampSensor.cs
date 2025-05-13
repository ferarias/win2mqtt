namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor("TimestampSensor")]
    public class TimestampSensor : ISensor
    {
        public Task<IDictionary<string, string>> CollectAsync() => Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string> { { "timestamp", DateTime.UtcNow.ToString("o") } });
    }
}
