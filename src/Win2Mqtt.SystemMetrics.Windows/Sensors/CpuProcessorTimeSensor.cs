using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor(
    "cpuprocessortime",
    name: "CPU Processor Time",
    unitOfMeasurement: "ms",
    deviceClass: "cpu",
    stateClass: "measurement")]
    public class CpuProcessorTimeSensor(ILogger<CpuProcessorTimeSensor> logger) : AttributedSensorBase<double>
    {

        public override Task<SensorValue<double>> CollectAsync()
        {
            var value = GetProcessorTime();
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(new SensorValue<double>(Metadata.Key, value));
        }

        private double GetProcessorTime()
        {
            throw new NotImplementedException();
        }
    }
}