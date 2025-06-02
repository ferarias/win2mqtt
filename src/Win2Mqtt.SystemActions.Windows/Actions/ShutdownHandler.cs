namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class ShutdownHandler : MqttActionHandler
    {
        private static readonly int DefaultShutdownDelay = 10;

        public override Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            if (int.TryParse(payload, out int shutdownDelay))
                WindowsPowerManagement.Shutdown(shutdownDelay);
            else
                WindowsPowerManagement.Shutdown(DefaultShutdownDelay);

            return Task.CompletedTask;
        }
    }

}
