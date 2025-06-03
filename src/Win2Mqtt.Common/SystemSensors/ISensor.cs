using System.Threading.Tasks;

namespace Win2Mqtt.SystemSensors
{
    public interface ISensor { }

    public interface ISensor<T> : ISensor
    {
        SensorMetadata Metadata { get; }
        Task<T> CollectAsync();
    }



}