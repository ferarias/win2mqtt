using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SensorFactory : ISensorFactory
    {
        private readonly IEnumerable<ISensor> _individualSensors;
        private readonly IEnumerable<IMultiSensor> _multiSensors;
        private readonly Win2MqttOptions _options;
        private readonly IServiceProvider _serviceProvider;


        public SensorFactory(
        IEnumerable<ISensor> individualSensors,
        IEnumerable<IMultiSensor> multiSensors,
        IOptions<Win2MqttOptions> options,
        IServiceProvider serviceProvider)
        {
            _individualSensors = individualSensors;
            _multiSensors = multiSensors;
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }


        public IEnumerable<ISensor> GetEnabledSensors()
        {
            foreach (var sensor in _individualSensors)
            {
                var key = sensor.GetType().Name;
                if (_options.Sensors.TryGetValue(key, out var config) && config.Enabled)
                {
                    yield return sensor;
                }
            }

            foreach (var multi in _multiSensors)
            {
                var key = multi.GetType().Name;
                if (_options.MultiSensors.TryGetValue(key, out var config) && config.Enabled)
                {
                    foreach (var sensor in multi.CreateSensors(_serviceProvider))
                    {
                        var sensorKey = sensor.GetType().Name;
                        if (config.Sensors.TryGetValue(sensorKey, out var sensorCfg) && sensorCfg.Enabled)
                        {
                            yield return sensor;
                        }
                    }
                }
            }

        }
    }
}