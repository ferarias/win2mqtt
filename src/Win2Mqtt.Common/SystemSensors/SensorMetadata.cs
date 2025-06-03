using System;

namespace Win2Mqtt.SystemSensors
{
    public class SensorMetadata
    {
        // Required for internal use and HA discovery
        public string Key { get; init; } = default!;

        public string Name { get; init; } = default!;

        public string? UniqueId { get; set; }

        public string? StateTopic { get; set; }


        public Type ValueType { get; init; } = default!;

        // Optional but common fields
        public string? UnitOfMeasurement { get; init; }

        // Home Assistant-specific fields (optional)
        public string? DeviceClass { get; init; }
        public string? StateClass { get; init; }
        public bool IsBinary { get; init; }



    }
}
