using System.Collections.Generic;

namespace Win2Mqtt.SystemActions
{
    public interface ISystemActionFactory
    {
        public IDictionary<string, ISystemActionWrapper> GetEnabledActions();
    }
}
