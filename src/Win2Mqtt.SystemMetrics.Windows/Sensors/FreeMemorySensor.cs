using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor("freememory", name: "Free Memory", unitOfMeasurement: "MB", deviceClass: "memory", stateClass: "measurement")]
    public class FreeMemorySensor(ILogger<FreeMemorySensor> logger) : Sensor<double>
    {

        public override Task<SensorValue<double>> CollectAsync()
        {
            var value = GetFreeMemory();
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(new SensorValue<double>(Metadata.Key, value));
        }

        private double GetFreeMemory()
        {
            throw new NotImplementedException();
        }
    }
}