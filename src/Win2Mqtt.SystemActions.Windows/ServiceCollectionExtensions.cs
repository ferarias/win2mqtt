using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemActions(this IServiceCollection services) =>
            services
            .AddTransient<IIncomingMessagesProcessor, WindowsIncomingMessagesProcessor>();
    }
}
