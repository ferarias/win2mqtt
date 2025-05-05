using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IIncomingMessagesProcessor
    {
        Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default);
    }
}
