using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttSubscriber
    {
        Task<bool> SubscribeAsync(CancellationToken cancellationToken = default);
    }
}