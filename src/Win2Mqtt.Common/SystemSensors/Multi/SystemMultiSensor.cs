using System.Collections.Generic;
using System;

namespace Win2Mqtt.SystemSensors.Multi
{
    public abstract class SystemMultiSensor : ISystemMultiSensor
    {
        protected SystemMultiSensor()
        {
            Key = SensorMetadataFactory.GetKeyFromAttribute(this)!;
        }

        public string Key { get; set; }

        public abstract IEnumerable<ISystemSensor> CreateSensors(IServiceProvider serviceProvider);
    }
}
