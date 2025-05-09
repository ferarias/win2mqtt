using System.Collections.Generic;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    public interface ISensorDataCollector
    {
        Task<IDictionary<string, string>> CollectSystemDataAsync();
    }
}