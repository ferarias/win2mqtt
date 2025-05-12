using Microsoft.Extensions.Options;
using System.Reflection;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SensorFactory : ISensorFactory
    {
        private readonly IEnumerable<ISensor> _allSensors;
        private readonly SensorsOptions _sensorOptions;

        public SensorFactory(IEnumerable<ISensor> allSensors, IOptions<Win2MqttOptions> options)
        {
            _allSensors = allSensors;
            _sensorOptions = options.Value.Sensors;

        }

        public IEnumerable<ISensor> GetEnabledSensors()
        {
            foreach (var sensor in _allSensors)
            {
                var keyAttr = sensor.GetType().GetCustomAttribute<SensorKeyAttribute>();
                if (keyAttr == null) continue;

                var key = keyAttr.Key;

                // Reflectively check if the config key is enabled
                var prop = _sensorOptions.GetType().GetProperty(key);
                if (prop?.PropertyType == typeof(bool) && (bool)prop.GetValue(_sensorOptions) == true)
                {
                    yield return sensor;
                }
            }
        }
    }
}