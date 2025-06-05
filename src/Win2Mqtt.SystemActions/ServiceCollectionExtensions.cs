using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemActions(this IServiceCollection services) => services
            .AddSystemActionsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
    }
}
