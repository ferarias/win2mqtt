using System.Threading;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemActions
{
    public interface ISystemAction : ISystemActionWrapper
    {
        Task HandleAsync(string payload, CancellationToken cancellationToken);
    }

    public interface ISystemAction<T> : ISystemActionWrapper
    {
        Task<T> HandleAsync(string payload, CancellationToken cancellationToken);
    }

}
