using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface ISystemAction
    {
        SystemActionMetadata Metadata { get; set; }

        Task<object?> HandleAsync(string payload, CancellationToken cancellationToken);
    }

}
