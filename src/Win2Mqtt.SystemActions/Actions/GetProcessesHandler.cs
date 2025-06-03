using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Actions
{
    public class GetProcessesHandler : SystemAction<ProcessInfo[]>
    {
        public override Task<ProcessInfo[]> HandleAsync(string payload, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                Process
                .GetProcesses()
                .Select(p => new ProcessInfo(p.ProcessName, p.Id))
                .ToArray());
        }
    }

    public record ProcessInfo(string Name, int Id);
}
