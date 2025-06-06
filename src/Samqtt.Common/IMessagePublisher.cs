using System.Threading;
using System.Threading.Tasks;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt
{
    public interface IMessagePublisher
    {
        Task PublishOnlineStatus(CancellationToken cancellationToken = default);

        Task PublishOfflineStatus(CancellationToken cancellationToken = default);

        Task PublishSensorDiscoveryMessage(SystemSensorMetadata sensor, CancellationToken cancellationToken = default);

        Task PublishSwitchDiscoveryMessage(SystemActionMetadata metadata, CancellationToken cancellationToken = default);

        Task PublishSensorValue(ISystemSensor sensor, object? value, CancellationToken cancellationToken = default);

    }
}