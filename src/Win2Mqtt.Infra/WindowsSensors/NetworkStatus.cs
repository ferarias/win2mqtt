using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemMetrics.Windows.WindowsSensors
{
    public static class NetworkStatus
    {
        public static bool IsNetworkAvailable() => NetworkInterface.GetIsNetworkAvailable();
    }
}
