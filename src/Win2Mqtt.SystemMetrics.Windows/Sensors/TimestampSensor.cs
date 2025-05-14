using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor(
    "timestamp",
    name: "System Timestamp",
    deviceClass: "timestamp",
    stateClass: "measurement")]
    public class TimestampSensor(ILogger<TimestampSensor> logger) : AttributedSensorBase<DateTime>
    {
        public override Task<SensorValue<DateTime>> CollectAsync()
        {
            var value = DateTime.UtcNow;
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(new SensorValue<DateTime>(Metadata.Key, value));
        }
    }
}
