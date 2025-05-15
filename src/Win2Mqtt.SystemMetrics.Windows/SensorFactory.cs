using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Common;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SensorFactory(
    IOptions<Win2MqttOptions> options,
    IServiceProvider serviceProvider,
    ILogger<SensorFactory> logger) : ISensorFactory
    {
        private readonly Win2MqttOptions _options = options.Value;

        public IEnumerable<ISensorWrapper> GetEnabledSensors()
        {
            var wrappers = new List<ISensorWrapper>();

            // Regular sensors
            var sensors = serviceProvider.GetServices<ISensor>();
            foreach (var sensor in sensors)
            {
                var sensorType = sensor.GetType();
                var iface = sensorType
                    .GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISensor<>));
                if (iface != null)
                {
                    var wrapperType = typeof(SensorWrapper<>).MakeGenericType(iface.GenericTypeArguments[0]);
                    if (Activator.CreateInstance(wrapperType, sensor) is ISensorWrapper wrapper)
                    {
                        if (_options.Sensors[wrapper.Metadata.Key].Enabled)
                        {
                            wrapper.Metadata.SensorUniqueId = $"{options.Value.DeviceUniqueId}_{SanitizeHelpers.Sanitize(wrapper.Metadata.Key)}";
                            wrapper.Metadata.SensorStateTopic = $"{options.Value.MqttBaseTopic}/{wrapper.Metadata.SensorUniqueId}";
                            wrappers.Add(wrapper);
                        }
                        else
                        {
                            logger.LogInformation("Sensor {sensor} is disabled in the configuration.", wrapper.Metadata.Key);
                        }
                    }
                }
            }

            // Multi-sensors
            var multiSensors = serviceProvider.GetServices<IMultiSensor>();
            foreach (var multi in multiSensors)
            {
                if (_options.MultiSensors[multi.Key].Enabled)
                {
                    var sensorsFromMulti = multi.CreateSensors(serviceProvider);
                    foreach (var sensor in sensorsFromMulti)
                    {
                        var iface = sensor.GetType()
                        .GetInterfaces()
                        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISensor<>));
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
                                    wrapper.Metadata.SensorUniqueId = $"{options.Value.DeviceUniqueId}_{SanitizeHelpers.Sanitize(wrapper.Metadata.Key)}";
                                    wrapper.Metadata.SensorStateTopic = $"{options.Value.MqttBaseTopic}/{wrapper.Metadata.SensorUniqueId}";
                                    wrappers.Add(wrapper);
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
            return wrappers;

        }
    }
}