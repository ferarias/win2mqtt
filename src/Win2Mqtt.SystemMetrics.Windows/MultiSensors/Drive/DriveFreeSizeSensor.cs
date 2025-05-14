using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ChildSensor("drive/{0}/sizefree",
    namePattern: "Drive {0} Free Space",
    unitOfMeasurement: "B",
    deviceClass: "storage",
    stateClass: "measurement")]
    public class DriveFreeSizeSensor(DriveInfo driveInfo, ILogger<DriveFreeSizeSensor> logger) 
        : ChildSensor<long>(driveInfo.Name.Replace(":\\", ""))
    {
        public override Task<SensorValue<long>> CollectAsync()
        {
            var value = driveInfo.AvailableFreeSpace;
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(new SensorValue<long>(Metadata.Key, value));
        }
    }
}
