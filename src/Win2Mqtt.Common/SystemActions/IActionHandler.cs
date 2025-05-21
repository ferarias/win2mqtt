using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface IMqttActionHandlerMarker { }

    public interface IMqttActionHandler : IMqttActionHandlerMarker
    {
        Task HandleAsync(string payload, CancellationToken cancellationToken);
    }

    public interface IMqttActionHandler<T> : IMqttActionHandlerMarker
    {
        Task<T> HandleAsync(string payload, CancellationToken cancellationToken);
    }
}
