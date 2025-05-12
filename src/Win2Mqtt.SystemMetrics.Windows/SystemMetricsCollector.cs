using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows
{

    public class SystemMetricsCollector : ISystemMetricsCollector
    {
        private readonly IEnumerable<ISensor> _sensors;
        private readonly ILogger<SystemMetricsCollector> logger;

        public SystemMetricsCollector(
            ISensorFactory sensorFactory,
            ILogger<SystemMetricsCollector> logger)
        {
            _sensors = sensorFactory.GetEnabledSensors();
            this.logger = logger;
        }

        public async Task<IDictionary<string, string>> CollectAsync()
        {
            logger.LogDebug("Collecting sensor data from system");
            var data = new Dictionary<string, string>();
            foreach (var sensor in _sensors)
            {
                try
                {
                    var sensorData = await sensor.CollectAsync();
                    foreach (var kvp in sensorData)
                    {
                        data[kvp.Key] = kvp.Value;
                    }
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