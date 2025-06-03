using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemSensors.Multi;

namespace Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive
{
    [SystemChildSensor("drive/{0}/sizetotal",
    namePattern: "Drive {0} Total Space",
    unitOfMeasurement: "B",
    deviceClass: "data_size",
    stateClass: "measurement")]
    public class DriveTotalSizeSensor(DriveInfo driveInfo, ILogger<DriveTotalSizeSensor> logger) 
        : SystemChildSensor<long>(driveInfo.Name.Replace(":\\", ""))
    {
        public override Task<long> CollectAsync()
        {
            var value = driveInfo.TotalSize;
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
