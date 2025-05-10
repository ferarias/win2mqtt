using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class CpuSensor : ISensor
    {
        private readonly ILogger<CpuSensor> _logger;

        public CpuSensor(ILogger<CpuSensor> logger)
        {
            _logger = logger;
        }

        public IDictionary<string, string> Collect()
        {
            try
            {
                var usage = GetProcessorTime();
                return new Dictionary<string, string>
                {
                    { "cpuprocessortime", usage.ToString(CultureInfo.InvariantCulture) }
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting CPU data");
                return new Dictionary<string, string>();
            }
        }

        private double GetProcessorTime()
        {
            throw new NotImplementedException();
        }
    }
}