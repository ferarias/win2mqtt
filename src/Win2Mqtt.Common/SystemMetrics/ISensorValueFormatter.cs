namespace Win2Mqtt.SystemMetrics
{
    public interface ISensorValueFormatter
    {
        public string Format<T>(T? value);
    }
}
