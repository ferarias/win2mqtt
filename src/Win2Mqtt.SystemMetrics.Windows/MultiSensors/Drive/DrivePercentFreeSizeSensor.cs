using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemMetrics.Windows.MultiSensors.Drive
{
    [ChildSensor("drive/{0}/percentfree",
    namePattern: "Drive {0} Percent Free",
    unitOfMeasurement: "%",
    stateClass: "measurement")]
    public class DrivePercentFreeSizeSensor(DriveInfo driveInfo, ILogger<DrivePercentFreeSizeSensor> logger) 
        : ChildSensor<double>(driveInfo.Name.Replace(":\\", ""))
    {
        public override Task<double> CollectAsync()
        {
            var value = Math.Round((double)driveInfo.TotalFreeSpace / driveInfo.TotalSize * 100, 1);
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
