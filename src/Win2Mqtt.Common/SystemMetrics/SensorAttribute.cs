using System;

namespace Win2Mqtt.SystemMetrics
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SensorAttribute(string key) : Attribute
    {
        public string Key { get; } = key;
    }
}
