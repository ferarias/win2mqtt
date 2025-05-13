using System.Globalization;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [Sensor("DriveFreeSizeSensor")]
    public class DriveFreeSizeSensor(DriveInfo driveInfo) : ISensor
    {
        public Task<IDictionary<string, string>> CollectAsync()
        {
            var dic = new Dictionary<string, string>
            {
                [$"drive/{driveInfo.Name.Replace(":\\", "")}/sizefree"] = driveInfo.AvailableFreeSpace.ToString(CultureInfo.InvariantCulture)
            };
            return Task.FromResult<IDictionary<string, string>>(dic);
        }
    }
}
