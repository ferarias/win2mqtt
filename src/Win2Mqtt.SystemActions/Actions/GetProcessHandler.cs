using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Actions
{
    public class GetProcessHandler : SystemAction<bool>
    {
        public override Task<bool> HandleAsync(string payload, CancellationToken cancellationToken)
        {
            try
            {
                var isRunning = Process.GetProcessesByName(payload)?.Length != 0 == true;
                return Task.FromResult(isRunning);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
