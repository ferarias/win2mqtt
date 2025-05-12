using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Win2Mqtt.SystemMetrics.Windows
{
    [SensorKey("MemorySensor")]
    public class MemorySensor : ISensor
    {
        private readonly ILogger<MemorySensor> _logger;

        public MemorySensor(ILogger<MemorySensor> logger)
        {
            _logger = logger;
        }

        public Task<IDictionary<string, string>> CollectAsync()
        {
            try
            {
                var freeMemory = GetFreeMemory();
                return Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string>
            {
                { "freememory", freeMemory.ToString(CultureInfo.InvariantCulture) }
            });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting memory data");
                return Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string>());
            }
        }

        private double GetFreeMemory()
        {
            throw new NotImplementedException();
        }
    }
}