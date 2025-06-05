using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive;

namespace Win2Mqtt.SystemSensors.Windows.MultiSensors
{
    public class DrivesMultiSensor() : SystemMultiSensorBase
    {
        public override IEnumerable<string> ChildIdentifiers => 
            DriveInfo.GetDrives()
            .Where(di => di.IsReady && di.DriveType != DriveType.Network)
            .Select(di => di.Name.Replace(":\\", ""));

        public override void RegisterChildSensors(IServiceCollection services)
        {
            foreach (var id in ChildIdentifiers)
            {
                services.AddKeyedSingleton<ISystemSensor, DriveFreeSizeSensor>($"{nameof(DriveFreeSizeSensor)}_{id}");
                services.AddKeyedSingleton<ISystemSensor, DrivePercentFreeSizeSensor>($"{nameof(DrivePercentFreeSizeSensor)}_{id}");
                services.AddKeyedSingleton<ISystemSensor, DriveTotalSizeSensor>($"{nameof(DriveTotalSizeSensor)}_{id}");
            }

        }
    }

}
