using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public abstract class SystemMultiSensorBase : ISystemMultiSensor
    {
        public required SystemSensorMetadata Metadata { get; set; }

        public virtual IEnumerable<string> ChildIdentifiers => [];
    }
}
