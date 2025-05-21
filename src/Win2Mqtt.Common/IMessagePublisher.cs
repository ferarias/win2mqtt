using System.Threading;
using System.Threading.Tasks;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt
{
    public interface IMessagePublisher
    {
        Task PublishOnlineStatus(CancellationToken cancellationToken = default);

        Task PublishOfflineStatus(CancellationToken cancellationToken = default);

        Task PublishSensorDiscoveryMessage(SensorMetadata sensor, CancellationToken cancellationToken = default);

        Task PublishSensorValue(ISensorWrapper sensor, object? value, CancellationToken cancellationToken = default);

    }
}