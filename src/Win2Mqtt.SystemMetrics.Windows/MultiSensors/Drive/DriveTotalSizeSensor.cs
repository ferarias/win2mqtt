using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ManualSensor("DriveTotalSizeSensor")]
    public class DriveTotalSizeSensor(DriveInfo driveInfo, ILogger<DriveTotalSizeSensor> logger) : ISensor<long>
    {
        public Task<SensorValue<long>> CollectAsync()
        {
            var key = $"drive/{driveInfo.Name.Replace(":\\", "")}/sizetotal";
            var value = driveInfo.TotalSize;
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<long>(key, value));
        }
    }
}
