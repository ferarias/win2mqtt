using System.Collections.Generic;

namespace Win2Mqtt.SystemActions
{
    public interface ISystemActionFactory
    {
        IDictionary<string, ISystemAction> GetEnabledActions();
    }

}
