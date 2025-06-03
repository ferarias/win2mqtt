using System.Threading.Tasks;

namespace Win2Mqtt.SystemSensors
{
    public interface ISystemSensor { }

    public interface ISystemSensor<T> : ISystemSensor
    {
        SystemSensorMetadata Metadata { get; }
        Task<T> CollectAsync();
    }



}