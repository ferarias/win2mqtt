using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SensorFactory(IOptions<Win2MqttOptions> options, ILoggerFactory loggerFactory) : ISensorFactory
    {
        public IEnumerable<ISensor> CreateSensors()
        {
            var sensors = new List<ISensor>();

            if (options.Value.Sensors.CpuSensor)
                sensors.Add(new CpuSensor(loggerFactory.CreateLogger<CpuSensor>()));

            if (options.Value.Sensors.FreeMemorySensor)
                sensors.Add(new MemorySensor(loggerFactory.CreateLogger<MemorySensor>()));

            if (options.Value.Sensors.DiskSensor)
                sensors.Add(new DiskSensor(loggerFactory.CreateLogger<DiskSensor>()));

            // and so on...

            return sensors;
        }
    }
}