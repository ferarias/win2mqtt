namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class HibernateHandler : SystemAction
    {
        public override Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            WindowsPowerManagement.HibernateSystem();
            return Task.CompletedTask;
        }
    }

}
