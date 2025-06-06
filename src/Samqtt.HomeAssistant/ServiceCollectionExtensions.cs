using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.HomeAssistant
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHomeAssistant(this IServiceCollection services) =>
            services
            .AddSingleton<ISystemSensorValueFormatter, HomeAssistantSensorValueFormatter>()
            .AddSingleton<IMessagePublisher, HomeAssistantPublisher>();
    }
}
