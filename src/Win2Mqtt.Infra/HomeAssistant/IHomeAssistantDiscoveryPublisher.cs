
namespace Win2Mqtt.Infra.HomeAssistant
{
    public interface IHomeAssistantDiscoveryPublisher
    {
        Task PublishSensorsDiscoveryAsync();
    }
}