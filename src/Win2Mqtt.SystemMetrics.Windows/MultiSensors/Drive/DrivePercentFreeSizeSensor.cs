using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ManualSensor("DrivePercentFreeSizeSensor")]
    public class DrivePercentFreeSizeSensor(DriveInfo driveInfo, ILogger<DrivePercentFreeSizeSensor> logger) : ISensor<double>
    {
        public Task<SensorValue<double>> CollectAsync()
        {
            var key = $"drive/{driveInfo.Name.Replace(":\\", "")}/percentfree";
            var value = Math.Round((double)driveInfo.TotalFreeSpace / driveInfo.TotalSize * 100, 1);
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<double>(key, value));
        }
    }
}
