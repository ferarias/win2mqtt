using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemMetrics.Windows.Sensors
{
    [Sensor(
    "networkavailability",
    name: "Network Availability",
    deviceClass: "connectivity",
    isBinary: true)]
    public class NetworkAvailabilitySensor(ILogger<NetworkAvailabilitySensor> logger) : Sensor<bool>
    {
        public override Task<SensorValue<bool>> CollectAsync()
        {
            var value = NetworkInterface.GetIsNetworkAvailable();
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(new SensorValue<bool>(Metadata.Key, value));
        }
    }
}
