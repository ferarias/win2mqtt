using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
namespace Win2Mqtt.SystemSensors
{
    public interface ISystemMultiSensor
    {
        SystemSensorMetadata Metadata { get; set; }
        
        IEnumerable<string> ChildIdentifiers { get; }

        void RegisterChildSensors(IServiceCollection services);
    }
}
