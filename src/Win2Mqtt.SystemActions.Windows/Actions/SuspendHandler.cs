namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class SuspendHandler : IMqttActionHandler
    {
        public Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            PowerManagement.SuspendSystem();
            return Task.CompletedTask;
        }
    }

}
