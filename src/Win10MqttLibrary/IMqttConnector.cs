using System;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttConnector
    {
        Task<bool> ConnectAsync();

        Task<bool> SubscribeAsync(Func<string, string, Task> processMessageAsync);

        Task PublishAsync(string topic, string message, bool retain);
        Task PublishForDeviceAsync(string subtopic, string message, bool retain = false);

        Task DisconnectAsync();
    }
}