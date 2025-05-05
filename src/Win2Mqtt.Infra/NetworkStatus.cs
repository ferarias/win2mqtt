using System.Net.NetworkInformation;

namespace Win2Mqtt.System.Metrics
{
    public static class NetworkStatus
    {
        public static bool IsNetworkAvailable() => NetworkInterface.GetIsNetworkAvailable();
    }
}
