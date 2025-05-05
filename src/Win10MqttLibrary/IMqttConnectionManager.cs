using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttConnectionManager
    {
        Task<bool> ConnectAsync();
        Task DisconnectAsync();
    }
}