using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor("TimestampSensor")]
    public class TimestampSensor(ILogger<TimestampSensor> logger) : ISensor<DateTime>
    {
        public Task<SensorValue<DateTime>> CollectAsync()
        {
            var key = "timestamp";
            var value = DateTime.UtcNow;
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<DateTime>(key, value));
        }
    }
}
