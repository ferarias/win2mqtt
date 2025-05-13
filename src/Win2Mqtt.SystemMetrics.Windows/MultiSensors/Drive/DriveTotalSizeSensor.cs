using System.Globalization;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [Sensor("DriveTotalSizeSensor")]
    public class DriveTotalSizeSensor(DriveInfo driveInfo) : ISensor
    {
        public Task<IDictionary<string, string>> CollectAsync()
        {
            var dic = new Dictionary<string, string>
            {
                [$"drive/{driveInfo.Name.Replace(":\\", "")}/sizetotal"] = driveInfo.TotalSize.ToString(CultureInfo.InvariantCulture)
            };
            return Task.FromResult<IDictionary<string, string>>(dic);
        }
    }
}
