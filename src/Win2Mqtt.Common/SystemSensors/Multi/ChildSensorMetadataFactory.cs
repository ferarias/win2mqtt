using System.Linq;
using System.Reflection;

namespace Win2Mqtt.SystemSensors.Multi
{
    public static class ChildSensorMetadataFactory
    {

        public static SystemSensorMetadata? FromChildSensor(ISystemSensor sensor, string id)
        {
            var type = sensor.GetType();
            var attr = type.GetCustomAttribute<SystemChildSensorAttribute>();
            if (attr == null) return null;

            var valueType = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystemSensor<>))
                ?.GenericTypeArguments[0] ?? typeof(object);

            return new SystemSensorMetadata
            {
                Key = string.Format(attr.KeyPattern, id),
                Name = attr.NamePattern == null ? string.Format(attr.KeyPattern, id) : string.Format(attr.NamePattern, id),
                ValueType = valueType,
                UnitOfMeasurement = attr.UnitOfMeasurement,
                DeviceClass = attr.DeviceClass,
                StateClass = attr.StateClass,
                IsBinary = attr.IsBinary
            };
        }

        public static string? GetKeyFromAttribute(ISystemMultiSensor multiSensor)
        {
            var type = multiSensor.GetType();
            var attr = type.GetCustomAttribute<SystemMultiSensorAttribute>();
            if (attr == null) return null;
            return attr.Key;
        }
    }
}
