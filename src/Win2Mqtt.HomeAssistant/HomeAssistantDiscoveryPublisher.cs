using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantDiscoveryPublisher(IHomeAssistantDiscoveryHelper homeAssistantDiscoveryHelper, IOptions<Win2MqttOptions> options)
        : IHomeAssistantDiscoveryPublisher
    {
        private readonly Win2MqttOptions _options = options.Value;

        public async Task PublishSensorsDiscoveryAsync(CancellationToken cancellationToken = default)
        {
            if (_options.Sensors.CpuSensor)
            {
                await homeAssistantDiscoveryHelper.PublishSensorDiscoveryAsync("cpuprocessortime", "CPU Usage", "%", null, "measurement", cancellationToken);
            }
            if (_options.Sensors.MemorySensor)
            {
                await homeAssistantDiscoveryHelper.PublishSensorDiscoveryAsync("freememory", "Free Memory", "MB", "data_size", "measurement", cancellationToken);
            }
            if (_options.Sensors.ComputerInUseSensor)
            {
                await homeAssistantDiscoveryHelper.PublishBinarySensorDiscoveryAsync("network_available", "Network Available", "connectivity", cancellationToken);
            }
            if (_options.Sensors.NetworkSensor)
            {
                await homeAssistantDiscoveryHelper.PublishBinarySensorDiscoveryAsync("inuse", "User Activity", "occupancy", cancellationToken);
            }
        }
    }
}
