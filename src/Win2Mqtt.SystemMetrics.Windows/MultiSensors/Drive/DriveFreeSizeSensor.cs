using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ManualSensor("drive/{0}/sizefree",
    name: "Drive {0} Free Space",
    unitOfMeasurement: "B",
    deviceClass: "storage",
    stateClass: "measurement")]
    public class DriveFreeSizeSensor(DriveInfo driveInfo, ILogger<DriveFreeSizeSensor> logger) : AttributedSensorBase<long>
    {
        public override Task<SensorValue<long>> CollectAsync()
        {
            var key = string.Format(Metadata.Key, driveInfo.Name.Replace(":\\", ""));

            var value = driveInfo.AvailableFreeSpace;
            logger.LogDebug("Collect {Key}: {Value}", key, value);
            return Task.FromResult(new SensorValue<long>(key, value));
        }
    }
}
