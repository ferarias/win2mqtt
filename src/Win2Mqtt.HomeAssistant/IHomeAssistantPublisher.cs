namespace Win2Mqtt.HomeAssistant
{
    public interface IHomeAssistantPublisher
    {
        Task PublishSensorsDiscoveryAsync(CancellationToken cancellationToken = default);

        Task NotifyOnlineStatus(CancellationToken cancellationToken = default);
        Task NotifyOfflineStatus(CancellationToken cancellationToken = default);
    }
}