using System.Linq;
using System.Reflection;

namespace Win2Mqtt.SystemMetrics
{
    public static class SensorMetadataFactory
    {
        public static SensorMetadata? FromSensor(ISensor sensor)
        {
            var type = sensor.GetType();
            var attr = type.GetCustomAttribute<SensorBaseAttribute>();
            if (attr == null) return null;

            var valueType = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISensor<>))
                ?.GenericTypeArguments[0] ?? typeof(object);

            return new SensorMetadata
            {
                Key = attr.Key,
                Name = attr.Name ?? attr.Key,
                ValueType = valueType,
                UnitOfMeasurement = attr.UnitOfMeasurement,
                DeviceClass = attr.DeviceClass,
                StateClass = attr.StateClass,
                IsBinary = attr.IsBinary
            };
        }
    }
}
