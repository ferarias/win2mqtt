using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemSensors
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add system sensors to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSystemSensors(this IServiceCollection services)
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

            // Temporarily build a provider to resolve all registered ISystemMultiSensor instances
            using var provider = services.BuildServiceProvider();
            var multiSensors = provider.GetServices<ISystemMultiSensor>();

            foreach (var sensor in multiSensors)
            {
                services.AddMultiSensorChildSensors(sensor);
            }

            return services;
        }

        private static IServiceCollection AddMultiSensorChildSensors(this IServiceCollection services, ISystemMultiSensor sensor)
        {
            Type type = sensor.GetType();
            var sensorBaseName = type.Name.Replace("MultiSensor", string.Empty);

            foreach (var id in sensor.ChildIdentifiers)
            {
                var sensorTypes = type.Assembly
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract &&
                        typeof(ISystemSensor).IsAssignableFrom(t) &&
                        t.Name.StartsWith(sensorBaseName, StringComparison.OrdinalIgnoreCase));

                foreach (var sensorType in sensorTypes)
                {
                    var key = $"{sensorType.Name}_{id}";
                    services.AddKeyedSingleton(typeof(ISystemSensor), sensorType, key);
                }
            }
            return services;
        }
    }
}
