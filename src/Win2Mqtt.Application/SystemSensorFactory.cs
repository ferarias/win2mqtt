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

        public IEnumerable<ISensorWrapper> GetEnabledSensors()
        {
            var sensors = new List<ISensorWrapper>();

            // Regular sensors
            var singleSensors = serviceProvider.GetServices<ISystemSensor>();
            foreach (var sensor in singleSensors)
            {
                var sensorType = sensor.GetType();
                var iface = sensorType
                    .GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystemSensor<>));
                if (iface != null)
                {
                    var wrapperType = typeof(SensorWrapper<>).MakeGenericType(iface.GenericTypeArguments[0]);
                    if (Activator.CreateInstance(wrapperType, sensor) is ISensorWrapper wrapper)
                    {
                        if (_options.Sensors[wrapper.Metadata.Key].Enabled)
                        {
                            wrapper.Metadata.UniqueId = $"{options.Value.DeviceUniqueId}_{SanitizeHelpers.Sanitize(wrapper.Metadata.Key)}";
                            wrapper.Metadata.StateTopic = $"{options.Value.MqttBaseTopic}/{wrapper.Metadata.UniqueId}";
                            sensors.Add(wrapper);
                        }
                        else
                        {
                            logger.LogInformation("Sensor {sensor} is disabled in the configuration.", wrapper.Metadata.Key);
                        }
                    }
                }
            }

            // Multi-sensors
            var multiSensors = serviceProvider.GetServices<ISystemMultiSensor>();
            foreach (var multi in multiSensors)
            {
                if (_options.MultiSensors[multi.Key].Enabled)
                {
                    var sensorsFromMulti = multi.CreateSensors(serviceProvider);
                    foreach (var sensor in sensorsFromMulti)
                    {
                        var iface = sensor.GetType()
                        .GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystemSensor<>));
                        if (iface != null)
                        {
                            var wrapperType = typeof(SensorWrapper<>).MakeGenericType(iface.GenericTypeArguments[0]);
                            if (Activator.CreateInstance(wrapperType, sensor) is ISensorWrapper wrapper)
                            {
                                var childSensorsOptions = _options.MultiSensors[multi.Key].Sensors;
                                var sensorName = sensor.GetType().Name;
                                if(sensorName.LastIndexOf("sensor", StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    sensorName = sensorName[..sensorName.LastIndexOf("sensor", StringComparison.OrdinalIgnoreCase)];
                                }
                                if (childSensorsOptions.TryGetValue(sensorName, out SensorOptions? value) && value.Enabled)
                                {
                                    wrapper.Metadata.UniqueId = $"{options.Value.DeviceUniqueId}_{SanitizeHelpers.Sanitize(wrapper.Metadata.Key)}";
                                    wrapper.Metadata.StateTopic = $"{options.Value.MqttBaseTopic}/{wrapper.Metadata.UniqueId}";
                                    sensors.Add(wrapper);
                                }
                                else
                                {
                                    logger.LogInformation("Sensor {sensor} is disabled in the configuration.", sensorName);
                                }
                            }
                                
                        }

                    }
                }
            }
            return sensors;

        }
    }
}