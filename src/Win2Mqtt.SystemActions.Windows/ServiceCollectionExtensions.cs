using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemActions.Windows.Actions;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindowsSystemActions(this IServiceCollection services) =>
            services
            .AddSingleton<HibernateHandler>()
            .AddSingleton<IMqttActionHandler, HibernateHandler>()
            .AddTransient<IIncomingMessagesProcessor, WindowsIncomingMessagesProcessor>();
    }
}
