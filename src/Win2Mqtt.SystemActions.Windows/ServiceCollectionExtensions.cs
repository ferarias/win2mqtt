using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemActions.Windows.Actions;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindowsSpecificSystemActions(this IServiceCollection services) =>
            services
            .AddSingleton<IMqttActionHandler, HibernateHandler>()
            .AddSingleton<IMqttActionHandler, RebootHandler>()
            .AddSingleton<IMqttActionHandler, ShutdownHandler>()
            .AddSingleton<IMqttActionHandler, SuspendHandler>();
    }
}
