using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{

    public class SystemMetricsCollector(
        IEnumerable<ISensor> sensors,
        ILogger<SystemMetricsCollector> logger)
        : ISystemMetricsCollector
    {
        private readonly ILogger<SystemMetricsCollector> _logger = logger;

        public Task<IDictionary<string, string>> CollectSystemDataAsync()
        {
            _logger.LogDebug("Collecting sensor data from system");
            var data = new Dictionary<string, string>();
            foreach (var sensor in sensors)
            {
                try
                {
                    var sensorData = sensor.Collect();
                    foreach (var kvp in sensorData)
                    {
                        data[kvp.Key] = kvp.Value;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error collecting sensor data");
                }
            }

            return Task.FromResult<IDictionary<string, string>>(data);
        }
    }
}