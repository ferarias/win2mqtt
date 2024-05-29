using System.Net.NetworkInformation;

namespace Win2Mqtt.Infra.HardwareSensors
{
    public static class NetworkStatus
    {
        public static bool IsNetworkAvailable() => NetworkInterface.GetIsNetworkAvailable();
    }
}
