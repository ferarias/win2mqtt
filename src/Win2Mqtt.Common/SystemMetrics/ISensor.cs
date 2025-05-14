using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    public interface ISensor { }

    public interface ISensor<T> : ISensor
    {
        SensorMetadata Metadata { get; }
        Task<SensorValue<T>> CollectAsync();
    }



}