using Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors
{
    [Sensor("DriveMultiSensor")]
    public class DriveMultiSensor() : IMultiSensor
    {
        public IEnumerable<ISensor> CreateSensors(IServiceProvider serviceProvider)
        {
            var drives = DriveInfo.GetDrives().Where(di => di.IsReady && di.DriveType != DriveType.Network);
            foreach (var item in drives)
            {
                yield return new DriveTotalSizeSensor(item);
                yield return new DriveFreeSizeSensor(item);
                yield return new DrivePercentFreeSizeSensor(item);
            }
        }
    }

}
