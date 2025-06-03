namespace Win2Mqtt.SystemSensors
{
    public interface ISensorValueFormatter
    {
        public string Format<T>(T? value);
        public T? Format<T>(string? value) where T : struct;
    }
}
