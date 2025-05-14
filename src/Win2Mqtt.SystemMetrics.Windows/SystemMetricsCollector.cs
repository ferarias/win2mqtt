using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SystemMetricsCollector(
        ISensorFactory sensorFactory,
        ISensorValueFormatter sensorValueFormatter,
        ILogger<SystemMetricsCollector> logger) : ISystemMetricsCollector
    {
        private readonly IEnumerable<ISensorWrapper> _sensors = sensorFactory.GetEnabledSensors();

        public async Task<IDictionary<string, string>> CollectAsync()
        {
            logger.LogDebug("Collecting sensor data from system");
            var data = new Dictionary<string, string>();

            foreach (var sensor in _sensors)
            {
                try
                {
                    var (key, value) = await sensor.CollectAsync();
                    data[key] = sensorValueFormatter.Format(value);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Error collecting sensor data");
                }
            }

            return data;
        }
    }
}