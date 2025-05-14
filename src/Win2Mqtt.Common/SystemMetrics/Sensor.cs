using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    public abstract class Sensor<T> : ISensor<T>
    {
        private readonly SensorMetadata _metadata;

        protected Sensor()
        {
            _metadata = SensorMetadataFactory.FromSensor(this)!;
        }

        public SensorMetadata Metadata => _metadata;

        public abstract Task<SensorValue<T>> CollectAsync();
    }

    public abstract class ChildSensor<T> : ISensor<T>
    {
        private readonly SensorMetadata _metadata;

        protected ChildSensor(string id)
        {
            _metadata = SensorMetadataFactory.FromChildSensor(this, id)!;
        }

        public SensorMetadata Metadata => _metadata;

        public abstract Task<SensorValue<T>> CollectAsync();
    }

    public abstract class MultiSensor : IMultiSensor
    {
        protected MultiSensor()
        {
            Key = SensorMetadataFactory.GetKeyFromAttribute(this)!;
        }

        public string Key { get; set; }

        public abstract IEnumerable<ISensor> CreateSensors(IServiceProvider serviceProvider);
    }
}
