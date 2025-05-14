using System;
using System.Collections.Generic;

namespace Win2Mqtt.SystemMetrics
{
    public interface IMultiSensor
    {
        public string Key { get; }
        IEnumerable<ISensor> CreateSensors(IServiceProvider serviceProvider);
    }

}
