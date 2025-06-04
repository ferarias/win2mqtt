using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemSensors.Windows.MultiSensors;
using Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive;
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

            services.AddSingleton<ISystemMultiSensor, DrivesMultiSensor>();

            DrivesMultiSensor drivesMultiSensor = new() { Metadata = new SystemSensorMetadata() };
            foreach (var id in drivesMultiSensor.ChildIdentifiers)
            {
                services.AddKeyedSingleton<ISystemSensor, DriveFreeSizeSensor>($"{nameof(DriveFreeSizeSensor)}_{id}");
                services.AddKeyedSingleton<ISystemSensor, DrivePercentFreeSizeSensor>($"{nameof(DrivePercentFreeSizeSensor)}_{id}");
                services.AddKeyedSingleton<ISystemSensor, DriveTotalSizeSensor>($"{nameof(DriveTotalSizeSensor)}_{id}");
            }

            return services;
        }
    }
}
