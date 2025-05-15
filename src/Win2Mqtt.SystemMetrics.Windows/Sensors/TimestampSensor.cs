using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor(
    "timestamp",
    name: "System Timestamp",
    deviceClass: "timestamp")]
    public class TimestampSensor(ILogger<TimestampSensor> logger) : Sensor<DateTime>
    {
        public override Task<DateTime> CollectAsync()
        {
            var value = DateTime.UtcNow;
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
