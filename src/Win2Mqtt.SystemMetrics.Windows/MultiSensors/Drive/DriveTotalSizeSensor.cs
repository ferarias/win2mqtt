using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ManualSensor("drive/{0}/sizetotal",
    name: "Drive {0} Total Space",
    unitOfMeasurement: "B",
    deviceClass: "storage",
    stateClass: "measurement")]
    public class DriveTotalSizeSensor(DriveInfo driveInfo, ILogger<DriveTotalSizeSensor> logger) : AttributedSensorBase<long>
    {
        public override Task<SensorValue<long>> CollectAsync()
        {
            var key = string.Format(Metadata.Key, driveInfo.Name.Replace(":\\", ""));
            var value = driveInfo.TotalSize;
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<long>(key, value));
        }
    }
}
