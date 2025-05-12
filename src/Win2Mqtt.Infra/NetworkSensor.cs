using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemMetrics.Windows
{
    [SensorKey("NetworkSensor")]
    public class NetworkSensor : ISensor
    {
        private readonly ILogger<NetworkSensor> _logger;

        public NetworkSensor(ILogger<NetworkSensor> logger)
        {
            _logger = logger;
        }

        public Task<IDictionary<string, string>> CollectAsync()
        {
            try
            {
                var isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
                return Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string>
                {
                    { "networkstatus", isNetworkAvailable ? "1" : "0" }
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting network data");
                return Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string>());
            }
        }
    }
}
