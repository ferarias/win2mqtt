namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class HibernateHandler : IMqttActionHandler
    {
        public Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            PowerManagement.HibernateSystem();
            return Task.CompletedTask;
        }
    }

}
