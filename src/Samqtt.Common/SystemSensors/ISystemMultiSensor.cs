using System.Collections.Generic;
namespace Win2Mqtt.SystemSensors
{
    public interface ISystemMultiSensor
    {       
        IEnumerable<string> ChildIdentifiers { get; }
    }
}
