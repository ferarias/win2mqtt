using System;
using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IMqttSubscriber
    {
        Task<bool> SubscribeAsync(Func<string, string, Task> processMessageAsync);
    }
}