using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface IMqttActionHandler
    {
        Task HandleAsync(string payload, CancellationToken cancellationToken);
    }

    public interface IMqttActionHandler<T>
    {
        Task<T> HandleAsync(string payload, CancellationToken cancellationToken);
    }
}
