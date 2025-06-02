using System.Collections.Generic;

namespace Win2Mqtt.SystemActions
{
    public interface IActionFactory
    {
        public IEnumerable<IMqttActionHandlerMarker> GetEnabledActions();
    }
}
