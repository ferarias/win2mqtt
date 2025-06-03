using System;

namespace Win2Mqtt.SystemSensors.Multi
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SystemChildSensorAttribute(
    string keyPattern,
    string? namePattern = null,
    string? unitOfMeasurement = null,
    string? deviceClass = null,
    string? stateClass = null,
    bool isBinary = false) : Attribute
    {
        public string KeyPattern { get; } = keyPattern;
        public string? NamePattern { get; } = namePattern;
        public string? UnitOfMeasurement { get; } = unitOfMeasurement;
        public string? DeviceClass { get; } = deviceClass;
        public string? StateClass { get; } = stateClass;
        public bool IsBinary { get; } = isBinary;
    }

}
