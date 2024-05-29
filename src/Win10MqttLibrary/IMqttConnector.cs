using System.Threading.Tasks;

namespace Win2Mqtt.Client.Mqtt
{
    public interface IMqttConnector
    {
        Task<bool> ConnectAsync();

        Task DisconnectAsync();

        bool Connected { get; }

        Task PublishMessageAsync(string topic, string message, bool retain = false);

        Task PublishRawAsync(string topic, byte[] bytes);
    }
}