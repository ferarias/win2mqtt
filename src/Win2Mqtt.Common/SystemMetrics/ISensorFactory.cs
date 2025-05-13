using System.Collections.Generic;

namespace Win2Mqtt.SystemMetrics
{
    public interface ISensorFactory
    {
        IEnumerable<ISensor> GetEnabledSensors();
    }
}
