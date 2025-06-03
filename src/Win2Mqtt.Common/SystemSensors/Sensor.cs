using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemSensors
{
    public abstract class Sensor<T> : ISystemSensor<T>
    {
        private readonly SystemSensorMetadata _metadata;

        protected Sensor()
        {
            _metadata = SensorMetadataFactory.FromSensor(this)!;
        }

        public SystemSensorMetadata Metadata => _metadata;

        public abstract Task<T> CollectAsync();
    }

    public abstract class ChildSensor<T> : ISystemSensor<T>
    {
        private readonly SystemSensorMetadata _metadata;

        protected ChildSensor(string id)
        {
            _metadata = SensorMetadataFactory.FromChildSensor(this, id)!;
        }

        public SystemSensorMetadata Metadata => _metadata;

        public abstract Task<T> CollectAsync();
    }

    public abstract class MultiSensor : ISystemMultiSensor
    {
        protected MultiSensor()
        {
            Key = SensorMetadataFactory.GetKeyFromAttribute(this)!;
        }

        public string Key { get; set; }

        public abstract IEnumerable<ISystemSensor> CreateSensors(IServiceProvider serviceProvider);
    }
}
