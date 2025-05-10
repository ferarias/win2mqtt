using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class SystemMetricsCollectorCollector(
        IEnumerable<ISensor> sensors,
        IOptions<Win2MqttOptions> options,
        ILogger<SystemMetricsCollectorCollector> logger)
        : ISystemMetricsCollector
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<SystemMetricsCollectorCollector> _logger = logger;

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