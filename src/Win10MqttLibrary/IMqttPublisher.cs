using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttPublisher
    {
        Task PublishAsync(string topic, string message, bool retain);
        Task PublishForDeviceAsync(string subtopic, string message, bool retain = false);
    }
}