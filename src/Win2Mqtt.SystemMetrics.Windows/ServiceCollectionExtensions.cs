using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add sensors for system metrics to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWindowsSystemMetrics(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblyOf<SensorFactory>()
            .AddClasses(c => c.WithAttribute<MultiSensorAttribute>().AssignableTo<IMultiSensor>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

            services.Scan(scan => scan
            .FromAssemblyOf<SensorFactory>()
            .AddClasses(c => c.WithAttribute<SensorAttribute>())
            .UsingRegistrationStrategy(RegistrationStrategy.Append)
            .As<ISensor>()
            .WithSingletonLifetime());

            services.AddSingleton<ISensorFactory, SensorFactory>();
            return services;
        }
    }
}
