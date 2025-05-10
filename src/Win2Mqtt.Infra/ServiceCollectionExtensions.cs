using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemMetrics(this IServiceCollection services, Win2MqttOptions options)
        {
            // Register the Windows-specific implementations
            services.AddSingleton<ISensor, TimeSensor>();

            if (options.Sensors.CpuSensor)
                services.AddSingleton<ISensor, CpuSensor>();

            if (options.Sensors.FreeMemorySensor)
                services.AddSingleton<ISensor, MemorySensor>();

            if (options.Sensors.DiskSensor)
                services.AddSingleton<ISensor, DiskSensor>();

            if (options.Sensors.NetworkSensor)
                services.AddSingleton<ISensor, NetworkSensor>();

            return services.AddTransient<ISystemMetricsCollector, SystemMetricsCollectorCollector>();
        }
    }
}
