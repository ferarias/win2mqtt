using System;
using System.Collections.Generic;

namespace Win2Mqtt.SystemSensors.Multi
{
    public interface ISystemMultiSensor
    {
        public string Key { get; set; }
        IEnumerable<ISystemSensor> CreateSensors(IServiceProvider serviceProvider);
    }

}
