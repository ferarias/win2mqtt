using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttPublisher
    {
        Task PublishAsync(string topic, string message, bool retain, CancellationToken cancellationToken = default);
        Task PublishForDeviceAsync(string subtopic, string message, bool retain = false, CancellationToken cancellationToken = default);
    }
}