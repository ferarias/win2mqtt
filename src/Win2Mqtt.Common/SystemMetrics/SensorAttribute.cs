using System;

namespace Win2Mqtt.SystemMetrics
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class SensorBaseAttribute(
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
    public sealed class SensorAttribute(
    string key,
    string? name = null,
    string? unitOfMeasurement = null,
    string? deviceClass = null,
    string? stateClass = null,
    bool isBinary = false)
    : SensorBaseAttribute(key, name, unitOfMeasurement, deviceClass, stateClass, isBinary)
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ManualSensorAttribute(
    string key,
    string? name = null,
    string? unitOfMeasurement = null,
    string? deviceClass = null,
    string? stateClass = null,
    bool isBinary = false)
    : SensorBaseAttribute(key, name, unitOfMeasurement, deviceClass, stateClass, isBinary)
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MultiSensorAttribute(string key) : Attribute
    {
        public string Key { get; } = key;
    }
}
