using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SensorFactory(IEnumerable<ISensor> allSensors, IOptions<Win2MqttOptions> options) : ISensorFactory
    {
        private readonly Dictionary<string, SensorOptions> _sensorOptions = options.Value.Sensors;

        public IEnumerable<ISensor> GetEnabledSensors()
        {
            foreach (var sensor in allSensors)
            {
                var key = sensor.GetType().Name;

                if (_sensorOptions[key].Enabled)
                {
                    yield return sensor;
                }
            }
        }
    }
}