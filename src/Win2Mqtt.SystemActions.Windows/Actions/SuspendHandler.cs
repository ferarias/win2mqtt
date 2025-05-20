namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class SuspendHandler : IMqttActionHandler
    {
        public Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            WindowsPowerManagement.SuspendSystem();
            return Task.CompletedTask;
        }
    }

}
