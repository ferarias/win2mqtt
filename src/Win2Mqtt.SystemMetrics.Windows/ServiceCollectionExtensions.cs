using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemMetrics(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<SensorFactory>()
            .AddClasses(classes => classes.AssignableTo<ISensor>())
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
            );

            services.Scan(scan => scan
            .FromAssemblyOf<SensorFactory>()
            .AddClasses(c => c.AssignableTo<IMultiSensor>())
            .As<IMultiSensor>()
            .WithSingletonLifetime());

            services.AddSingleton<ISensorFactory, SensorFactory>();
            services.AddSingleton<ISystemMetricsCollector, SystemMetricsCollector>();
            return services;
        }
    }
}
