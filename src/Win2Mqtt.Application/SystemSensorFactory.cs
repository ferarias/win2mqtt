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
            var sensors = new Dictionary<string, ISystemSensor>(StringComparer.OrdinalIgnoreCase);

            var allSensors = serviceProvider.GetServices<ISystemSensor>();

            foreach (var (sensorName, sensorOpts) in _options.Sensors)
            {
                if (!sensorOpts.Enabled)
                {
                    logger.LogInformation("Sensor {Sensor} is disabled in config.", sensorName);
                    continue;
                }
                // We expect a concrete type named e.g. "FreeMemorySensor" etc.
                var typeName = sensorName + "Sensor";
                var t = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(x =>
                          x.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                       && typeof(ISystemSensor).IsAssignableFrom(x));

                if (t is null)
                {
                    logger.LogWarning("No sensor type found for key `{Sensor}`", sensorName);
                    continue;
                }

                // Resolve the single ISystemSensor instance (registered in DI)
                var sensorInstance = allSensors.FirstOrDefault(s => t.IsInstanceOfType(s));
                if (sensorInstance is null)
                {
                    logger.LogWarning("DI could not resolve sensor `{Sensor}` of type {Type}", sensorName, t.FullName);
                    continue;
                }

                // Compute topic/uniqueId and IsBinary
                var topicName = string.IsNullOrWhiteSpace(sensorOpts.Topic)
                    ? SanitizeHelpers.Sanitize(sensorName)
                    : SanitizeHelpers.Sanitize(sensorOpts.Topic);

                var uniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(sensorName)}";

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

                sensorInstance.Metadata = new SystemSensorMetadata
                {
                    Key = sensorName,
                    Name = t.Name.Replace("Sensor", string.Empty),
                    UniqueId = uniqueId,
                    StateTopic = $"{_options.MqttBaseTopic}/{topicName}",
                    IsBinary = isBinary
                };

                sensors.Add(sensorName, sensorInstance);
            }

            return sensors;
        }
    }
}