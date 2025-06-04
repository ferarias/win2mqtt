using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemSensors.Multi;

namespace Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive
{
    [HomeAssistantSensor(unitOfMeasurement: "%", stateClass: "measurement")]
    public class DrivePercentFreeSizeSensor(DriveInfo driveInfo, ILogger<DrivePercentFreeSizeSensor> logger) 
        : SystemChildSensor<double>(driveInfo.Name.Replace(":\\", ""))
    {
        public override Task<double> CollectAsync()
        {
            var value = Math.Round((double)driveInfo.TotalFreeSpace / driveInfo.TotalSize * 100, 1);
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
