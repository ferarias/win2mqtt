namespace Win2Mqtt.SystemMetrics
{
    public record struct SensorValue<T>(string Key, T Value)
    {
        public static implicit operator (string Key, T Value)(SensorValue<T> value)
        {
            return (value.Key, value.Value);
        }

        public static implicit operator SensorValue<T>((string Key, T Value) value)
        {
            return new SensorValue<T>(value.Key, value.Value);
        }
    }

}