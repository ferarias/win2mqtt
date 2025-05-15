using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMessageListener
    {
        Task SubscribeToIncomingMessagesAsync(CancellationToken stoppingToken);
    }
}
