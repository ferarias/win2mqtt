using System.Threading.Tasks;

namespace Win2Mqtt.SystemSensors.Multi
{
    public abstract class SystemChildSensor<T> : ISystemSensor<T>
    {
        public required SystemSensorMetadata Metadata { get; set; }

        public abstract Task<T> CollectAsync();

        protected SystemChildSensor(string id)
        {
            
            Metadata = new SystemSensorMetadata { Key = id };
        }
    }
}
