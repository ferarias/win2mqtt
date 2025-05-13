using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SensorFactory(
    IOptions<Win2MqttOptions> options,
    IServiceProvider serviceProvider) : ISensorFactory
    {
        //TODO check if enabled from options
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
                        wrappers.Add(wrapper);
                }
            }

            // Multi-sensors
            var multiSensors = serviceProvider.GetServices<IMultiSensor>();
            foreach (var multi in multiSensors)
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
                            wrappers.Add(wrapper);
                    }
                }
            }
            return wrappers;

        }
    }
}