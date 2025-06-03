using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemSensors.Multi;

namespace Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive
{
    [SystemChildSensor("drive/{0}/sizefree",
    namePattern: "Drive {0} Free Space",
    unitOfMeasurement: "B",
    deviceClass: "data_size",
    stateClass: "measurement")]
    public class DriveFreeSizeSensor(DriveInfo driveInfo, ILogger<DriveFreeSizeSensor> logger) 
        : SystemChildSensor<long>(driveInfo.Name.Replace(":\\", ""))
    {
        public override Task<long> CollectAsync()
        {
            var value = driveInfo.AvailableFreeSpace;
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
