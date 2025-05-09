using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemMetrics(this IServiceCollection services) =>
            services
            .AddTransient<ISensorDataCollector, SensorDataCollector>();
    }
}
