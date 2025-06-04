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

                var typeName = sensorName + "Sensor";
                var t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(x =>
                          x.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                       && typeof(ISystemSensor).IsAssignableFrom(x));

                if (t is null)
                {
                    logger.LogWarning("No sensor type named `{Type}` exists in AppDomain assemblies", typeName);
                    continue;
                }

                var sensorInstance = allSensors.FirstOrDefault(s => t.IsInstanceOfType(s));
                if (sensorInstance is null)
                {
                    logger.LogWarning("DI could not resolve sensor `{Sensor}` of type {Type}", sensorName, t.FullName);
                    continue;
                }

                var topicName = string.IsNullOrWhiteSpace(sensorOpts.Topic)
                    ? SanitizeHelpers.Sanitize(sensorName)
                    : SanitizeHelpers.Sanitize(sensorOpts.Topic);

                var uniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(sensorName)}";
                var isBinary = ReturnsBinaryValue(t);

                sensorInstance.Metadata = new SystemSensorMetadata
                {
                    Key = sensorName,
                    Name = t.Name.Replace("Sensor", string.Empty),
                    UniqueId = uniqueId,
                    StateTopic = $"{_options.MqttBaseTopic}/{topicName}",
                    IsBinary = isBinary
                };

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

                // We expect a concrete type named e.g. "DriveMultiSensor" etc.
                var typeName = multisensorName + "MultiSensor";
                var t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(x =>
                          x.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                       && typeof(ISystemMultiSensor).IsAssignableFrom(x));

                if (t is null)
                {
                    logger.LogWarning("No multi-sensor type named `{Type}` exists in AppDomain assemblies", typeName);
                    continue;
                }

                var multisensorInstance = allMultiSensors.FirstOrDefault(s => t.IsInstanceOfType(s));
                if (multisensorInstance is null)
                {
                    logger.LogWarning("DI could not resolve multi-sensor `{Sensor}` of type {Type}", multisensorName, t.FullName);
                    continue;
                }

                var topicName = string.IsNullOrWhiteSpace(multisensorOpts.Topic)
                    ? SanitizeHelpers.Sanitize(multisensorName)
                    : SanitizeHelpers.Sanitize(multisensorOpts.Topic);

                multisensorInstance.Metadata = new SystemSensorMetadata
                {
                    Key = multisensorName,
                    Name = t.Name.Replace("MultiSensor", string.Empty),
                    UniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(multisensorName)}",
                    StateTopic = $"{_options.MqttBaseTopic}/{topicName}",
                    IsBinary = false // Multi-sensors are not binary by default
                };
                yield return new KeyValuePair<string, ISystemMultiSensor>(multisensorName, multisensorInstance);
            }
        }

        private IEnumerable<KeyValuePair<string, ISystemSensor>> GetMultiSensorChildren(string parentMultisensorName, ISystemMultiSensor parentMultiSensor)
        {
            foreach (var (sensorName, sensorOpts) in _options.MultiSensors[parentMultisensorName].Sensors)
            {
                if (!sensorOpts.Enabled)
                {
                    logger.LogInformation("Multi-sensor child sensor {Sensor} is disabled in config.", sensorName);
                    continue;
                }

                var typeName = sensorName + "Sensor";
                var t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(x =>
                          x.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                       && typeof(ISystemSensor).IsAssignableFrom(x));

                if (t is null)
                {
                    logger.LogWarning("No sensor type named `{Type}` exists in AppDomain assemblies (child sensor of {Sensor})", typeName, parentMultisensorName);
                    continue;
                }

                foreach (var id in parentMultiSensor.ChildIdentifiers)
                {
                    var instanceSensorName = $"{typeName}_{id}";
                    var childSensorInstance = serviceProvider.GetKeyedService<ISystemSensor>(instanceSensorName);
                    if (childSensorInstance is null)
                    {
                        logger.LogWarning("DI could not resolve child sensor `{Sensor}` of type {Type} (child sensor of {Sensor})", sensorName, t.FullName, parentMultisensorName);
                        continue;
                    }

                    var childTopicName = string.IsNullOrWhiteSpace(sensorOpts.Topic)
                    ? $"{parentMultiSensor.Metadata.StateTopic}_{SanitizeHelpers.Sanitize(instanceSensorName)}"
                    : $"{parentMultiSensor.Metadata.StateTopic}_{SanitizeHelpers.Sanitize(sensorOpts.Topic)}";
                    var uniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(instanceSensorName)}";
                    var isBinary = ReturnsBinaryValue(t);

                    childSensorInstance.Metadata = new SystemSensorMetadata
                    {
                        Key = instanceSensorName,
                        InstanceId = id,
                        Name = t.Name.Replace("Sensor", string.Empty),
                        UniqueId = uniqueId,
                        StateTopic = $"{_options.MqttBaseTopic}/{childTopicName}",
                        IsBinary = isBinary
                    };
                    yield return new KeyValuePair<string, ISystemSensor>(instanceSensorName, childSensorInstance);
                }
               
            }
        }



        private static bool ReturnsBinaryValue(Type t)
        {
            // Look up the generic argument (T) of SystemSensorBase<T> to see if it's bool
            // or any other type, so we know IsBinary.
            var baseIface = t.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystemSensor));
            // Actually, since we removed the generic interface, just test if CollectAsync returns bool:
            var isBinary = false;
            var method = t.GetMethod("CollectInternalAsync", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method?.ReturnType.IsGenericType == true
                && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                && method.ReturnType.GetGenericArguments()[0] == typeof(bool))
            {
                isBinary = true;
            }

            return isBinary;
        }
    }
}