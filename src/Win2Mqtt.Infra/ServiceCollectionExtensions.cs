using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.Common;

namespace Win2Mqtt.System.Metrics
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemMetrics(this IServiceCollection services) =>
            services
            .AddTransient<ISensorDataCollector, SensorDataCollector>();
    }
}
