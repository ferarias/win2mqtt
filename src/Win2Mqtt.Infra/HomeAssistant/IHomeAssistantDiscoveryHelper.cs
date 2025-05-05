
namespace Win2Mqtt.Infra.HomeAssistant
{
    public interface IHomeAssistantDiscoveryHelper
    {
        Task PublishBinarySensorDiscoveryAsync(string sensorKey, string name, string? deviceClass = null, CancellationToken cancellationToken = default);
        Task PublishSensorDiscoveryAsync(string sensorKey, string name, string? unit = null, string? deviceClass = null, string? stateClass = null, CancellationToken cancellationToken = default);
        Task UnpublishSensorDiscoveryAsync(string sensorKey, bool isBinary = false, CancellationToken cancellationToken = default);
    }
}