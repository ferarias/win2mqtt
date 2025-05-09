using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static class NetworkStatus
    {
        public static bool IsNetworkAvailable() => NetworkInterface.GetIsNetworkAvailable();
    }
}
