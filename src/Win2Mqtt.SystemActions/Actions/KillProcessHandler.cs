using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Actions
{
    public class KillProcessHandler : SystemAction<bool>
    {
        public override Task<bool> HandleAsync(string payload, CancellationToken cancellationToken)
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
