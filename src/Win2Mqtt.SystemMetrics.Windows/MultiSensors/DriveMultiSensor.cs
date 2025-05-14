using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors
{
    [MultiSensor("drives")]
    public class DriveMultiSensor() : MultiSensor
    {
        public override IEnumerable<ISensor> CreateSensors(IServiceProvider serviceProvider)
        {
            var drives = DriveInfo.GetDrives().Where(di => di.IsReady && di.DriveType != DriveType.Network);
            foreach (var item in drives)
            {
                yield return new DriveTotalSizeSensor(item, serviceProvider.GetRequiredService<ILogger<DriveTotalSizeSensor>>());
                yield return new DriveFreeSizeSensor(item, serviceProvider.GetRequiredService<ILogger<DriveFreeSizeSensor>>());
                yield return new DrivePercentFreeSizeSensor(item, serviceProvider.GetRequiredService<ILogger<DrivePercentFreeSizeSensor>>());
            }
        }
    }

}
