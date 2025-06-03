using System.Threading.Tasks;

namespace Win2Mqtt.SystemSensors.Multi
{
    public abstract class SystemChildSensor<T> : ISystemSensor<T>
    {
        private readonly SystemSensorMetadata _metadata;

        protected SystemChildSensor(string id)
        {
            _metadata = SensorMetadataFactory.FromChildSensor(this, id)!;
        }

        public SystemSensorMetadata Metadata => _metadata;

        public abstract Task<T> CollectAsync();
    }
}
