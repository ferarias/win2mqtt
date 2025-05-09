using System;
using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttSubscriber
    {
        Task<bool> SubscribeAsync(Func<string, string, CancellationToken, Task> processMessageAsync, CancellationToken cancellationToken = default);
    }
}