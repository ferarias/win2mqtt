using System.Threading.Tasks;

namespace Win2Mqtt.SystemSensors
{
    public abstract class SystemSensor<T> : ISystemSensor<T>
    {
        private readonly SystemSensorMetadata _metadata;

        protected SystemSensor()
        {
            _metadata = SensorMetadataFactory.FromSensor(this)!;
        }

        public SystemSensorMetadata Metadata => _metadata;

        public abstract Task<T> CollectAsync();
    }
}
