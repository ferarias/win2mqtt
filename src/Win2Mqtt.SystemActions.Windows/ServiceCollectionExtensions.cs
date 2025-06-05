using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindowsSpecificSystemActions(this IServiceCollection services) => services
            .AddSystemActionsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
    }
}
