using System.Collections.Generic;

namespace Win2Mqtt.SystemMetrics
{
    public interface ISensor
    {
        /// <summary>
        /// Returns one or more topic/value pairs.
        /// </summary>
        IDictionary<string, string> Collect();
    }
}
