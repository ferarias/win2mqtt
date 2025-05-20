namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class RebootHandler : IMqttActionHandler
    {
        private static readonly int DefaultRebootDelay = 10;

        public Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            if (int.TryParse(payload, out int rebootDelay))
                PowerManagement.Reboot(rebootDelay);
            else
                PowerManagement.Reboot(DefaultRebootDelay);

            return Task.CompletedTask;
        }
    }

}
