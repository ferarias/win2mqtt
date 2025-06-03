using System;
using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors
{
    public interface ISystemMultiSensor
    {
        public string Key { get; }
        IEnumerable<ISystemSensor> CreateSensors(IServiceProvider serviceProvider);
    }

}
