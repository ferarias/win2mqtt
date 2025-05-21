using System;
using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public interface IMultiSensor
    {
        public string Key { get; }
        IEnumerable<ISensor> CreateSensors(IServiceProvider serviceProvider);
    }

}
