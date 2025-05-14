using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ManualSensor("drive/{0}/percentfree",
    name: "Drive {0} Percent Free",
    unitOfMeasurement: "%",
    deviceClass: "storage",
    stateClass: "measurement")]
    public class DrivePercentFreeSizeSensor(DriveInfo driveInfo, ILogger<DrivePercentFreeSizeSensor> logger) : AttributedSensorBase<double>
    {
        public override Task<SensorValue<double>> CollectAsync()
        {
            var key = string.Format(Metadata.Key, driveInfo.Name.Replace(":\\", ""));
            var value = Math.Round((double)driveInfo.TotalFreeSpace / driveInfo.TotalSize * 100, 1);
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<double>(key, value));
        }
    }
}
