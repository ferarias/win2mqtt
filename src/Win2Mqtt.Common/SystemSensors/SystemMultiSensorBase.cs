using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemSensors
{
    public abstract class SystemMultiSensorBase : ISystemMultiSensor
    {
        public required SystemSensorMetadata Metadata { get; set; }

        public virtual IEnumerable<string> ChildIdentifiers => [];

        public abstract void RegisterChildSensors(IServiceCollection services);
    }
}
