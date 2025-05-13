using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor("CpuProcessorTimeSensor")]
    public class CpuProcessorTimeSensor(ILogger<CpuProcessorTimeSensor> logger) : ISensor<double>
    {
        public Task<SensorValue<double>> CollectAsync()
        {
            var key = "cpuprocessortime";
            var value = GetProcessorTime();
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<double>(key, value));
        }

        private double GetProcessorTime()
        {
            throw new NotImplementedException();
        }
    }
}