using System.Collections.Generic;
namespace Win2Mqtt.SystemSensors
{
    public interface ISystemMultiSensor
    {
        SystemSensorMetadata Metadata { get; set; }
        
        IEnumerable<string> ChildIdentifiers { get; }
    }
}
