using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.HomeAssistant
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomeAssistantDiscovery(this IServiceCollection services) =>
            services
            .AddSingleton<IHomeAssistantDiscoveryHelper, HomeAssistantDiscoveryHelper>()
            .AddSingleton<IHomeAssistantDiscoveryPublisher, HomeAssistantDiscoveryPublisher>();
    }
}
