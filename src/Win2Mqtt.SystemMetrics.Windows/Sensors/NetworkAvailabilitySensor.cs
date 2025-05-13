using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor("NetworkAvailabilitySensor")]
    public class NetworkAvailabilitySensor(ILogger<NetworkAvailabilitySensor> logger) : ISensor<bool>
    {
        public Task<SensorValue<bool>> CollectAsync()
        {
            var key = "networkavailability";
            var value = NetworkInterface.GetIsNetworkAvailable();
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<bool>(key, value));
        }
    }
}
