using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor("FreeMemorySensor")]
    public class FreeMemorySensor(ILogger<FreeMemorySensor> logger) : ISensor<double>
    {
        public Task<SensorValue<double>> CollectAsync()
        {
            var key = "freememory";
            var value = GetFreeMemory();
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<double>(key, value));
        }

        private double GetFreeMemory()
        {
            throw new NotImplementedException();
        }
    }
}