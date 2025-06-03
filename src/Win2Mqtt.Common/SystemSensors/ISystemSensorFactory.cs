using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public interface ISystemSensorFactory
    {
        IEnumerable<ISystemSensorWrapper> GetEnabledSensors();
    }
}
