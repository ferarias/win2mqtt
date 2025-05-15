using System.Threading;
using System.Threading.Tasks;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt
{
    public interface IMessagePublisher
    {
        Task NotifyOnlineStatus(CancellationToken cancellationToken = default);

        Task NotifyOfflineStatus(CancellationToken cancellationToken = default);

        Task PublishSensorDiscoveryMessage(ISensorWrapper sensor, CancellationToken cancellationToken = default);

        Task PublishSensorValue(ISensorWrapper sensor, object? value, CancellationToken cancellationToken = default);

    }
}