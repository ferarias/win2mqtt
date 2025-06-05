using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemSensors.Windows
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add system sensors to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWindowsSpecificSensors(this IServiceCollection services)
        {
            services
                .Scan(scan => scan
                    .FromAssembliesOf([typeof(ServiceCollectionExtensions)])
                    .AddClasses(c => c.AssignableTo<ISystemSensor>(), publicOnly: true)
                    .As<ISystemSensor>()
                    .WithSingletonLifetime())
                .Scan(scan => scan
                    .FromAssembliesOf([typeof(ServiceCollectionExtensions)])
                    .AddClasses(c => c.AssignableTo<ISystemMultiSensor>(), publicOnly: true)
                    .As<ISystemMultiSensor>()
                    .WithSingletonLifetime());

            // We need a temporary service provider to resolve multi-sensors and register their child sensors.
            using var provider = services.BuildServiceProvider();
            var multiSensors = provider.GetServices<ISystemMultiSensor>();
            foreach (var sensor in multiSensors)
            {
                sensor.RegisterChildSensors(services);
            }

            return services;
        }
    }
}
