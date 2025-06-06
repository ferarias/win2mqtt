using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public abstract class SystemMultiSensorBase : ISystemMultiSensor
    {
        public virtual IEnumerable<string> ChildIdentifiers => [];
    }
}
