using System;

namespace Win2Mqtt.SystemMetrics
{
    public class SensorMetadata
    {
        // Required for internal use and HA discovery
        public string Key { get; init; } = default!;
        public string Name { get; init; } = default!;
        public Type ValueType { get; init; } = default!;

        // Optional but common fields
        public string? UnitOfMeasurement { get; init; }

        // Home Assistant-specific fields (optional)
        public string? DeviceClass { get; init; }
        public string? StateClass { get; init; }
        public bool IsBinary { get; init; }


        public string? SensorUniqueId { get; set; }
        public string? SensorStateTopic { get; set; }

    }
}
