using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface IIncomingMessagesProcessor
    {
        Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default);
    }
}
