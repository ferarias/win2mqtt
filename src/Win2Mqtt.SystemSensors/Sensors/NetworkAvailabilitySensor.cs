﻿using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;

namespace Win2Mqtt.SystemSensors.Sensors
{
    [Sensor(
    "networkavailability",
    name: "Network Availability",
    deviceClass: "connectivity",
    isBinary: true)]
    public class NetworkAvailabilitySensor(ILogger<NetworkAvailabilitySensor> logger) : Sensor<bool>
    {
        public override Task<bool> CollectAsync()
        {
            var value = NetworkInterface.GetIsNetworkAvailable();
            logger.LogDebug("Collect {Key}: {Value}", Metadata.Key, value);
            return Task.FromResult(value);
        }
    }
}
