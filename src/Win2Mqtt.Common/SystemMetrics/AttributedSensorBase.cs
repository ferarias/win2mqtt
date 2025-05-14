using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    public abstract class AttributedSensorBase<T> : ISensor<T>
    {
        private readonly SensorMetadata _metadata;

        protected AttributedSensorBase()
        {
            _metadata = SensorMetadataFactory.FromSensor(this)!;
        }

        public SensorMetadata Metadata => _metadata;

        public abstract Task<SensorValue<T>> CollectAsync();
    }
}
