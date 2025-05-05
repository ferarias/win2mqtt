using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.Common
{
    public interface IMqttPublisher
    {
        Task PublishAsync(string topic, string message, bool retain, CancellationToken cancellationToken = default);
    }
}