using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{

    public abstract class SystemAction : ISystemAction
    {
        public required SystemActionMetadata Metadata { get; set; }

        public abstract Task HandleAsync(string payload, CancellationToken cancellationToken);
    }


    public abstract class SystemAction<T> : ISystemAction<T>
    {
        public required SystemActionMetadata Metadata { get; set; }

        public abstract Task<T> HandleAsync(string payload, CancellationToken cancellationToken);
    }
}
