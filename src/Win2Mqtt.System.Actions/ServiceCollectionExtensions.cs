using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.Common;

namespace Win2Mqtt.System.Actions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemActions(this IServiceCollection services) =>
            services
            .AddTransient<IIncomingMessagesProcessor, IncomingMessagesProcessor>();
    }
}
