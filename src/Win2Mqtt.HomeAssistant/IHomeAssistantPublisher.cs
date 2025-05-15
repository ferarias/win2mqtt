namespace Win2Mqtt.HomeAssistant
{
    public interface IHomeAssistantPublisher
    {
        Task NotifyOnlineStatus(CancellationToken cancellationToken = default);
        Task NotifyOfflineStatus(CancellationToken cancellationToken = default);
    }
}