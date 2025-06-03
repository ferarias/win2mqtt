using System.Linq;
using System.Reflection;

namespace Win2Mqtt.SystemSensors
{
    public static class SensorMetadataFactory
    {
        public static SystemSensorMetadata? FromSensor(ISystemSensor sensor)
        {
            var type = sensor.GetType();
            var attr = type.GetCustomAttribute<SystemSensorAttribute>();
            if (attr == null) return null;

            var valueType = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystemSensor<>))
                ?.GenericTypeArguments[0] ?? typeof(object);

            return new SystemSensorMetadata
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
