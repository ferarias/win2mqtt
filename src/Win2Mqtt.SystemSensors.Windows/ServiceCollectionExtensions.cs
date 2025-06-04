using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemSensors.Windows.Sensors;

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
            services.AddSingleton<ISystemSensor, CpuProcessorTimeSensor>();
            services.AddSingleton<ISystemSensor, FreeMemorySensor>();


            return services;
        }
    }
}
