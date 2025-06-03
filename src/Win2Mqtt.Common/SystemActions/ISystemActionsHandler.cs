using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface ISystemActionsHandler
    {
        Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default);
    }
}
