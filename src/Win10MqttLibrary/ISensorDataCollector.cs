using System.Collections.Generic;
using System.Threading.Tasks;

namespace Win2Mqtt.Client.Mqtt
{
    public interface ISensorDataCollector
    {
        Task<IDictionary<string, string>> CollectSystemDataAsync();
    }
}