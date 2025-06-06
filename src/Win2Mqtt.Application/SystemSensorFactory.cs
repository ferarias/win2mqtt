using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Common;
using Win2Mqtt.Options;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public class SystemSensorFactory(
        IOptions<Win2MqttOptions> options,
        IServiceProvider serviceProvider,
        ILogger<SystemSensorFactory> logger) : ISystemSensorFactory
    {
        private readonly Win2MqttOptions _options = options.Value;

        public IDictionary<string, ISystemSensor> GetEnabledSensors()
        {
            var sensors = GetSimpleSensors().ToDictionary();

            foreach (var (multisensorName, multisensorInstance) in GetMultiSensors())
            {
                foreach (var (childSensorName, childSensorInstance) in GetMultiSensorChildren(multisensorName, multisensorInstance))
                {
                    sensors.Add(childSensorName, childSensorInstance);
                }
            }

            return sensors;
        }

        private IEnumerable<KeyValuePair<string, ISystemSensor>> GetSimpleSensors()
        {
            var allSensors = serviceProvider.GetServices<ISystemSensor>();
            foreach (var (sensorName, sensorOpts) in _options.Sensors)
            {
                if (!sensorOpts.Enabled)
                {
                    logger.LogInformation("Sensor {Sensor} is disabled in config.", sensorName);
                    continue;
                }

                var t = ResolveSensorType<ISystemSensor>(sensorName, "Sensor");
                if (t is null)
                {
                    logger.LogWarning("No sensor type named `{Type}` exists in AppDomain assemblies", sensorName + "Sensor");
                    continue;
                }

                var sensorInstance = allSensors.FirstOrDefault(s => t.IsInstanceOfType(s));
                if (sensorInstance is null)
                {
                    logger.LogWarning("DI could not resolve sensor `{Sensor}` of type {Type}", sensorName, t.FullName);
                    continue;
                }

                var topicName = SanitizeTopicOrDefault(sensorName, sensorOpts.Topic);
                var uniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(sensorName)}";
                var isBinary = ReturnsBinaryValue(t);

                
                sensorInstance.Metadata = CreateMetadata(
                    sensorName,
                    t.Name.Replace("Sensor", string.Empty),
                    topicName,
                    uniqueId,
                    isBinary
                );


                yield return new KeyValuePair<string, ISystemSensor>(sensorName, sensorInstance);
            }
        }

        private IEnumerable<KeyValuePair<string, ISystemMultiSensor>> GetMultiSensors()
        {
            var allMultiSensors = serviceProvider.GetServices<ISystemMultiSensor>();

            foreach (var (multisensorName, multisensorOpts) in _options.MultiSensors)
            {
                if (!multisensorOpts.Enabled)
                {
                    logger.LogInformation("MultiSensor {Sensor} is disabled in config.", multisensorName);
                    continue;
                }

                var t = ResolveSensorType<ISystemMultiSensor>(multisensorName, "MultiSensor");
                if (t is null)
                {
                    logger.LogWarning("No multi-sensor type named `{Type}` exists in AppDomain assemblies", multisensorName + "MultiSensor");
                    continue;
                }

                var multisensorInstance = allMultiSensors.FirstOrDefault(s => t.IsInstanceOfType(s));
                if (multisensorInstance is null)
                {
                    logger.LogWarning("DI could not resolve multi-sensor `{Sensor}` of type {Type}", multisensorName, t.FullName);
                    continue;
                }

                yield return new KeyValuePair<string, ISystemMultiSensor>(multisensorName, multisensorInstance);
            }
        }

        private IEnumerable<KeyValuePair<string, ISystemSensor>> GetMultiSensorChildren(string parentMultisensorName, ISystemMultiSensor parentMultiSensor)
        {
            var multiOpts = _options.MultiSensors[parentMultisensorName];

            foreach (var (sensorName, sensorOpts) in multiOpts.Sensors)
            {
                if (!sensorOpts.Enabled)
                {
                    logger.LogInformation("Multi-sensor child sensor {Sensor} is disabled in config.", sensorName);
                    continue;
                }

                var t = ResolveSensorType<ISystemSensor>(sensorName, "Sensor");
                if (t is null)
                {
                    logger.LogWarning(
                        "No sensor type named `{Type}` exists in AppDomain assemblies (child sensor of {Parent})",
                        sensorName + "Sensor",
                        parentMultisensorName
                    );
                    continue;
                }

                foreach (var id in parentMultiSensor.ChildIdentifiers)
                {
                    var instanceSensorName = $"{sensorName}Sensor_{id}";
                    var childSensorInstance = serviceProvider.GetKeyedService<ISystemSensor>(instanceSensorName);
                    if (childSensorInstance is null)
                    {
                        logger.LogWarning(
                            "DI could not resolve child sensor `{Sensor}` of type {Type} (child sensor of {Parent})",
                            sensorName, t.FullName, parentMultisensorName
                        );
                        continue;
                    }

                    var baseTopic = SanitizeTopicOrDefault(parentMultisensorName, multiOpts.Topic);
                    var childTopicFragment = SanitizeTopicOrDefault(instanceSensorName, sensorOpts.Topic);
                    var childTopicName = $"{baseTopic}_{childTopicFragment}";
                    var uniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(instanceSensorName)}";
                    var isBinary = ReturnsBinaryValue(t);

                    childSensorInstance.Metadata = CreateMetadata(
                        key: instanceSensorName,
                        name: t.Name.Replace("Sensor", string.Empty),
                        topic: childTopicName,
                        uniqueId: uniqueId,
                        isBinary: isBinary,
                        instanceId: id
                    );

                    yield return new KeyValuePair<string, ISystemSensor>(instanceSensorName, childSensorInstance);
                }
               
            }
        }

        private SystemSensorMetadata CreateMetadata<T>(string sensorName, string defaultSensorTopicName, string? instanceId = null) where T : ISystemSensor
        {
            Type sensorType = typeof(T);
            var isBinary = ReturnsBinaryValue(sensorType);

            var sm = new SystemSensorMetadata
            {
                Key = sensorName,
                Name = sensorType.Name.Replace("Sensor", string.Empty),
                UniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(sensorName)}",
                StateTopic = $"{_options.MqttBaseTopic}/{SanitizeTopicOrDefault(sensorName, defaultSensorTopicName)}",
                IsBinary = isBinary,
                InstanceId = instanceId
            };
            if (Attribute.GetCustomAttribute(sensorType, typeof(HomeAssistantSensorAttribute)) is HomeAssistantSensorAttribute haAttr)
            {
                sm.UnitOfMeasurement = haAttr.UnitOfMeasurement;
                sm.DeviceClass = haAttr.DeviceClass;
                sm.StateClass = haAttr.StateClass;
            }
            return sm;
        }

        private SystemSensorMetadata CreateMetadata(string key, string name, string topic, string uniqueId, bool isBinary, string? instanceId = null)
        {
            return new SystemSensorMetadata
            {
                Key = key,
                Name = name,
                UniqueId = uniqueId,
                StateTopic = $"{_options.MqttBaseTopic}/{topic}",
                IsBinary = isBinary,
                InstanceId = instanceId
            };
        }

        private static Type? ResolveSensorType<T>(string baseName, string suffix)
        {
            var typeName = baseName + suffix;
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(x =>
                    x.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                    && typeof(T).IsAssignableFrom(x));
        }

        private static string SanitizeTopicOrDefault(string fallback, string? topic) => 
            SanitizeHelpers.Sanitize(string.IsNullOrWhiteSpace(topic) ? fallback : topic);


        private static bool ReturnsBinaryValue(Type t)
        {
            var method = t.GetMethod("CollectInternalAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method?.ReturnType.IsGenericType == true
                && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                && method.ReturnType.GetGenericArguments()[0] == typeof(bool))
            {
                return true;
            }

            return false;
        }
    }
}