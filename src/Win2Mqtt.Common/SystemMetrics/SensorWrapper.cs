using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    /// <summary>
    /// Non-Generic Wrapper Interface
    /// A non-generic abstraction that allows calling CollectAsync() uniformly
    /// </summary>
    public interface ISensorWrapper
    {
        Task<(string Key, string Value)> CollectAsync();
    }

    /// <summary>
    /// Implementation if ISensorWrapper(Of T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sensor"></param>
    public class SensorWrapper<T>(ISensor<T> sensor) : ISensorWrapper
    {
        public async Task<(string Key, string Value)> CollectAsync()
        {
            var (key, value) = await sensor.CollectAsync();
            //TODO You can improve formatting later (e.g., format DateTime or round double)
            return (key, value?.ToString() ?? string.Empty);
        }
    }

}