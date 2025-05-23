﻿using Microsoft.Extensions.Logging;

namespace Win2Mqtt.SystemSensors.Windows.MultiSensors.Drive
{
    [ChildSensor("drive/{0}/sizefree",
    namePattern: "Drive {0} Free Space",
    unitOfMeasurement: "B",
    deviceClass: "data_size",
    stateClass: "measurement")]
    public class DriveFreeSizeSensor(DriveInfo driveInfo, ILogger<DriveFreeSizeSensor> logger) 
        : ChildSensor<long>(driveInfo.Name.Replace(":\\", ""))
    {
        public override Task<long> CollectAsync()
        {
            var value = driveInfo.AvailableFreeSpace;
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
