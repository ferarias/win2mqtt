using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ManualSensor("DriveFreeSizeSensor")]
    public class DriveFreeSizeSensor(DriveInfo driveInfo, ILogger<DriveFreeSizeSensor> logger) : ISensor<long>
    {
        public Task<SensorValue<long>> CollectAsync()
        {
            var key = $"drive/{driveInfo.Name.Replace(":\\", "")}/sizefree";
            var value = driveInfo.AvailableFreeSpace;
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<long>(key, value));
        }
    }
}
