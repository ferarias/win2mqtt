using System;

namespace Win2Mqtt.SystemSensors.Multi
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SystemMultiSensorAttribute(string key) : Attribute
    {
        public string Key { get; } = key;
    }

}
