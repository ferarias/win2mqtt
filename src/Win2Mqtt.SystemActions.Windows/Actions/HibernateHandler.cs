namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class HibernateHandler : IMqttActionHandler
    {
        public Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            WindowsPowerManagement.HibernateSystem();
            return Task.CompletedTask;
        }
    }

}
