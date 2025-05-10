using System.Collections.Generic;
using System.Threading.Tasks;

namespace Win2Mqtt.SystemMetrics
{
    public interface ISystemMetricsCollector
    {
        Task<IDictionary<string, string>> CollectSystemDataAsync();
    }
}