using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemActions.Windows.Actions;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindowsSystemActions(this IServiceCollection services) =>
            services
            .AddSingleton<IMqttActionHandler<ProcessInfo[]>, GetProcessesHandler>()
            .AddSingleton<IMqttActionHandler<bool>, GetProcessHandler>()
            .AddSingleton<IMqttActionHandler, HibernateHandler>()
            .AddSingleton<IMqttActionHandler<bool>, KillProcessHandler>()
            .AddSingleton<IMqttActionHandler, RebootHandler>()
            .AddSingleton<IMqttActionHandler, ShutdownHandler>()
            .AddSingleton<IMqttActionHandler, StartProcessHandler>()
            .AddSingleton<IMqttActionHandler, SuspendHandler>()
            .AddSingleton<IMqttActionHandler, HibernateHandler>();
    }
}
