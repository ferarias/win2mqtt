using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor(
    "cpuprocessortime",
    name: "CPU Processor Time",
    unitOfMeasurement: "%",
    deviceClass: "",
    stateClass: "measurement")]
    public class CpuProcessorTimeSensor(ILogger<CpuProcessorTimeSensor> logger) : Sensor<double>
    {
        public override async Task<SensorValue<double>> CollectAsync()
        {
            var cpuUsage = await GetCpuUsageAsync();
            logger.LogDebug("Collect {Key}: {Value}%", Metadata.Key, cpuUsage);
            return new SensorValue<double>(Metadata.Key, cpuUsage);

        }

        private static async Task<double> GetCpuUsageAsync(int delayMilliseconds = 500)
        {
            using var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            // The first call always returns 0, so we discard it
            _ = cpuCounter.NextValue();

            await Task.Delay(delayMilliseconds);

            var usage = cpuCounter.NextValue();
            return Math.Round(usage, 1); // 1 decimal precision
        }

    }
}