using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWin2MqttApplication(this IServiceCollection services) =>
            services
            .AddSingleton<ISensorFactory, SensorFactory>();
    }
}
