using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemMetrics(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<SystemMetricsCollector>() 
            .AddClasses(classes => classes.AssignableTo<ISensor>())
            .As<ISensor>()
            .WithSingletonLifetime()
            );

            services.AddSingleton<ISensorFactory, SensorFactory>();
            services.AddSingleton<ISystemMetricsCollector, SystemMetricsCollector>();
            return services;
        }
    }
}
