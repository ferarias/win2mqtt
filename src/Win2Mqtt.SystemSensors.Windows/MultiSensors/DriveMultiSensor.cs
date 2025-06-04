using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemSensors.Multi;
using Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive;

namespace Win2Mqtt.SystemSensors.Windows.MultiSensors
{
    public class DriveMultiSensor() : SystemMultiSensor
    {
        public override IEnumerable<ISystemSensor> CreateSensors(IServiceProvider serviceProvider)
        {
            var sensors = new List<ISystemSensor>();
            var drives = DriveInfo.GetDrives().Where(di => di.IsReady && di.DriveType != DriveType.Network);
            foreach (var item in drives)
            {
                //yield return new DriveTotalSizeSensor(item, serviceProvider.GetRequiredService<ILogger<DriveTotalSizeSensor>>());
                //yield return new DriveFreeSizeSensor(item, serviceProvider.GetRequiredService<ILogger<DriveFreeSizeSensor>>());
                //yield return new DrivePercentFreeSizeSensor(item, serviceProvider.GetRequiredService<ILogger<DrivePercentFreeSizeSensor>>());
            }
            return sensors;
        }
    }

}
