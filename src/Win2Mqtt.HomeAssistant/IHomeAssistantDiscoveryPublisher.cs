namespace Win2Mqtt.HomeAssistant
{
    public interface IHomeAssistantDiscoveryPublisher
    {
        Task PublishSensorsDiscoveryAsync(CancellationToken cancellationToken = default);
    }
}