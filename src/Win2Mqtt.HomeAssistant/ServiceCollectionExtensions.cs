using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.HomeAssistant
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomeAssistantDiscovery(this IServiceCollection services) =>
            services
            .AddSingleton<ISensorValueFormatter, HomeAssistantSensorValueFormatter>()
            .AddSingleton<IMessagePublisher, HomeAssistantPublisher>();
    }
}
