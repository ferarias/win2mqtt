using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class KillProcessHandler : IMqttActionHandler<bool>
    {
        public Task<bool> HandleAsync(string payload, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(payload))
                {
                    process.Kill();
                }
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
