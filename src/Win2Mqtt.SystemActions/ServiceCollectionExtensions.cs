using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemActions.Actions;

namespace Win2Mqtt.SystemActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemActions(this IServiceCollection services) =>
            services
            .AddSingleton<IMqttActionHandler<ProcessInfo[]>, GetProcessesHandler>()
            .AddSingleton<IMqttActionHandler<bool>, GetProcessHandler>()
            .AddSingleton<IMqttActionHandler<bool>, KillProcessHandler>()
            .AddSingleton<IMqttActionHandler, StartProcessHandler>();
    }
}
