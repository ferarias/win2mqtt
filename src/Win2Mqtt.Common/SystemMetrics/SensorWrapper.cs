using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    /// <summary>
    /// Non-Generic Wrapper Interface
    /// A non-generic abstraction that allows calling CollectAsync() uniformly
    /// </summary>
    public interface ISensorWrapper
    {
        SensorMetadata Metadata { get; }
        Task<(string Key, object? Value)> CollectAsync();
    }

    /// <summary>
    /// Implementation if ISensorWrapper(Of T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sensor"></param>
    public class SensorWrapper<T>(ISensor<T> sensor) : ISensorWrapper
    {
        public SensorMetadata Metadata => sensor.Metadata;

        public async Task<(string, object?)> CollectAsync()
        {
            return await sensor.CollectAsync();
        }

    }

}