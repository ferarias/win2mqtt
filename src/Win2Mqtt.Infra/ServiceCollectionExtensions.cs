using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemMetrics(this IServiceCollection services)
        {
            services.AddSingleton<ISensorFactory, SensorFactory>();
            services.AddSingleton<ISystemMetricsCollector>(sp =>
            {
                var factory = sp.GetRequiredService<ISensorFactory>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                return new SystemMetricsCollector(factory.CreateSensors(), loggerFactory.CreateLogger<SystemMetricsCollector>());
            });
            return services;
        }
    }
}
