using Microsoft.Extensions.Logging;
using System.Globalization;
using Win2Mqtt.SystemMetrics.Windows.WindowsSensors;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class MemorySensor : ISensor
    {
        private readonly ILogger<MemorySensor> _logger;

        public MemorySensor(ILogger<MemorySensor> logger)
        {
            _logger = logger;
        }

        public IDictionary<string, string> Collect()
        {
            try
            {
                var freeMemory = Memory.GetFreeMemory();
                return new Dictionary<string, string>
            {
                { "freememory", freeMemory.ToString(CultureInfo.InvariantCulture) }
            };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting memory data");
                return new Dictionary<string, string>();
            }
        }
    }
}