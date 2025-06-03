using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Win2Mqtt.SystemSensors.Multi;

namespace Win2Mqtt.SystemSensors
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add system sensors to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSystemSensors(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblies([typeof(ServiceCollectionExtensions).Assembly])
            .AddClasses(c => c.WithAttribute<SystemMultiSensorAttribute>().AssignableTo<ISystemMultiSensor>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

            services.Scan(scan => scan
            .FromAssemblies([typeof(ServiceCollectionExtensions).Assembly])
            .AddClasses(c => c.WithAttribute<SystemSensorAttribute>())
            .UsingRegistrationStrategy(RegistrationStrategy.Append)
            .As<ISystemSensor>()
            .WithSingletonLifetime());

            return services;
        }
    }
}
