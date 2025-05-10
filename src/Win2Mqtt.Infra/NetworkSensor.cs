using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class NetworkSensor : ISensor
    {
        private readonly ILogger<NetworkSensor> _logger;

        public NetworkSensor(ILogger<NetworkSensor> logger)
        {
            _logger = logger;
        }

        public IDictionary<string, string> Collect()
        {
            try
            {
                var isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
                return new Dictionary<string, string>
                {
                    { "networkstatus", isNetworkAvailable ? "1" : "0" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting network data");
                return new Dictionary<string, string>();
            }
        }
    }
}
