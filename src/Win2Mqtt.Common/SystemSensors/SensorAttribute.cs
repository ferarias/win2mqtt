using System;

namespace Win2Mqtt.SystemSensors
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SensorAttribute(
    string key,
    string? name = null,
    string? unitOfMeasurement = null,
    string? deviceClass = null,
    string? stateClass = null,
    bool isBinary = false) : Attribute
    {
        public string Key { get; } = key;
        public string? Name { get; } = name;
        public string? UnitOfMeasurement { get; } = unitOfMeasurement;
        public string? DeviceClass { get; } = deviceClass;
        public string? StateClass { get; } = stateClass;
        public bool IsBinary { get; } = isBinary;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MultiSensorAttribute(string key) : Attribute
    {
        public string Key { get; } = key;
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ChildSensorAttribute(
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
