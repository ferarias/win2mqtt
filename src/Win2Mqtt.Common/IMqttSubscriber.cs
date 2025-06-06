using System;
using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttSubscriber
    {
        Task<bool> SubscribeAsync(string topic, Func<string, CancellationToken, Task<object?>> handler, CancellationToken cancellationToken = default);
    }
}