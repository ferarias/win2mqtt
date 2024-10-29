using System.Threading.Tasks;

namespace Win2Mqtt.Client.Mqtt
{
    public interface IMqttConnector
    {
        Task<bool> ConnectAsync();
        Task<bool> SubscribeAsync();

        Task DisconnectAsync();

        Task PublishMessageAsync(string topic, string message, bool retain = false);

        Task PublishRawAsync(string topic, byte[] bytes);
    }
}