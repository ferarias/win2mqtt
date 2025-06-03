using System.Collections.Generic;

namespace Win2Mqtt.SystemActions
{
    public interface IActionFactory
    {
        public IDictionary<string, IMqttActionHandlerMarker> GetEnabledActions();
    }
}
