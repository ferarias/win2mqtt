using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemSensors.Sensors;

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
            services.AddSingleton<ISystemSensor, NetworkAvailabilitySensor>(); // if FreeMemorySensor implements ISystemSensorWrapper
            services.AddSingleton<ISystemSensor, TimestampSensor>(); // if FreeMemorySensor implements ISystemSensorWrapper
            return services;
        }
    }
}
