namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class SuspendHandler : SystemAction
    {
        public override Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            WindowsPowerManagement.SuspendSystem();
            return Task.CompletedTask;
        }
    }

}
