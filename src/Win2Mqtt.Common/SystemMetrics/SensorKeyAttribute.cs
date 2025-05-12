using System;

namespace Win2Mqtt.SystemMetrics
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SensorKeyAttribute : Attribute
    {
        public string Key { get; }
        public SensorKeyAttribute(string key) => Key = key;
    }

}
