namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class HibernateHandler : MqttActionHandler
    {
        public override Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            WindowsPowerManagement.HibernateSystem();
            return Task.CompletedTask;
        }
    }

}
