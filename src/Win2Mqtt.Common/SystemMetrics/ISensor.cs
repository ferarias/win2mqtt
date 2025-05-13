using System.Collections.Generic;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    public interface ISensor
    {
        /// <summary>
        /// Returns one or more topic/value pairs.
        /// </summary>
        Task<IDictionary<string, string>> CollectAsync();
    }
}
