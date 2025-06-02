using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface IMqttActionHandlerMarker
    { 
        public SwitchMetadata Metadata { get; set; }
    }

    public interface IMqttActionHandler : IMqttActionHandlerMarker
    {
        Task HandleAsync(string payload, CancellationToken cancellationToken);
    }

    public interface IMqttActionHandler<T> : IMqttActionHandlerMarker
    {
        Task<T> HandleAsync(string payload, CancellationToken cancellationToken);
    }

    public abstract class MqttActionHandler<T> : IMqttActionHandler<T>
    {
        public required SwitchMetadata Metadata { get; set; }

        public abstract Task<T> HandleAsync(string payload, CancellationToken cancellationToken);
    }
    public abstract class MqttActionHandler : IMqttActionHandler
    {
        public required SwitchMetadata Metadata { get; set; }

        public abstract Task HandleAsync(string payload, CancellationToken cancellationToken);
    }
}
