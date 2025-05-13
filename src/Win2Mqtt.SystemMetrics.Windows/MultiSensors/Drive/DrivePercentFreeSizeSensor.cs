using System.Globalization;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [Sensor("DrivePercentFreeSizeSensor")]
    public class DrivePercentFreeSizeSensor(DriveInfo driveInfo) : ISensor
    {
        public Task<IDictionary<string, string>> CollectAsync()
        {
            var dic = new Dictionary<string, string>
            {
                [$"drive/{driveInfo.Name.Replace(":\\", "")}/percentfree"] = Math.Round((double)driveInfo.TotalFreeSpace / driveInfo.TotalSize * 100, 0).ToString(CultureInfo.InvariantCulture)
            };
            return Task.FromResult<IDictionary<string, string>>(dic);
        }
    }
}
