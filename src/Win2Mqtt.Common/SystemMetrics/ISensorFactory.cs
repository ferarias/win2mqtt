using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public interface ISensorFactory
    {
        IEnumerable<ISensorWrapper> GetEnabledSensors();
    }
}
