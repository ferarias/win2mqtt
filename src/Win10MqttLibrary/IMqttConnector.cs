using System;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttConnector
    {
        Task<bool> ConnectAsync();

        Task<bool> SubscribeAsync(Func<string, string, Task> processMessageAsync);

        Task PublishMessageAsync(string topic, string message, bool retain = false);

        Task PublishRawAsync(string topic, byte[] bytes);

        Task PublishToFullTopicAsync(string fullTopic, string message, bool retain);

        Task DisconnectAsync();
    }
}