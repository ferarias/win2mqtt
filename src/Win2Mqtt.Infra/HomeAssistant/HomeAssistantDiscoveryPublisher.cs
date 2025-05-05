using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.Infra.HomeAssistant
{
    public class HomeAssistantDiscoveryPublisher
    {
        private readonly Win2MqttOptions _options;
        private HomeAssistantDiscoveryHelper _discoveryHelper;

        public HomeAssistantDiscoveryPublisher(HomeAssistantDiscoveryHelper homeAssistantDiscoveryHelper, IOptions<Win2MqttOptions> options)
        {
            _discoveryHelper = homeAssistantDiscoveryHelper;
            _options = options.Value;
        }

        public async Task PublishSensorsDiscoveryAsync()
        {
            if (_options.Sensors.CpuSensor)
            {
                await _discoveryHelper.PublishSensorDiscoveryAsync("cpuprocessortime", "CPU Usage", "%", null, "measurement");

            }
            if (_options.Sensors.FreeMemorySensor)
            {
                await _discoveryHelper.PublishSensorDiscoveryAsync("freememory", "Free Memory", "MB", "data_size", "measurement");

            }
            if (_options.Sensors.IsComputerUsed)
            {
                await _discoveryHelper.PublishBinarySensorDiscoveryAsync("network_available", "Network Available", "connectivity");

            }
            if (_options.Sensors.NetworkSensor)
            {
                await _discoveryHelper.PublishBinarySensorDiscoveryAsync("inuse", "User Activity", "occupancy");
            }
        }

    }

}
