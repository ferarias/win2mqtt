using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public interface ISystemSensorFactory
    {
        public IDictionary<string, ISystemSensor> GetEnabledSensors();
    }
}
