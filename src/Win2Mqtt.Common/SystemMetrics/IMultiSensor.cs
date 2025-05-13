using System;
using System.Collections.Generic;

namespace Win2Mqtt.SystemMetrics
{
    public interface IMultiSensor
    {
        IEnumerable<ISensor> CreateSensors(IServiceProvider serviceProvider);
    }

}
