using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Windows.Actions
{
    public class GetProcessesHandler : IMqttActionHandler<ProcessInfo[]>
    {
        public Task<ProcessInfo[]> HandleAsync(string payload, CancellationToken cancellationToken)
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
