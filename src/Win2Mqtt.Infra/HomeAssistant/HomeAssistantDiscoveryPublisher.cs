using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.Infra.HomeAssistant
{
    public class HomeAssistantDiscoveryPublisher
    {
        private readonly Win2MqttOptions _options;
        private readonly string _baseTopic;
        private HomeAssistantDiscoveryHelper _haDiscovery;

        public HomeAssistantDiscoveryPublisher(HomeAssistantDiscoveryHelper homeAssistantDiscoveryHelper, IOptions<Win2MqttOptions> options)
        {
            _haDiscovery = homeAssistantDiscoveryHelper;
            _options = options.Value;
            _baseTopic = $"win2mqtt/{_options.MqttTopic}";
        }

        public async Task PublishSensorsDiscoveryAsync()
        {
            if (_options.Sensors.FreeMemorySensor)
            {
                await _haDiscovery.PublishSensorDiscoveryAsync("freememory", "Free Memory", "MB", "data_size", "measurement");

            }
            if (_options.Sensors.CpuSensor)
            {
                // No 'power_factor' here — use 'power' or drop it if not electrical. You could consider this "None".
                await _haDiscovery.PublishSensorDiscoveryAsync("cpuprocessortime", "CPU Usage", "%", null, "measurement");

            }
            if (_options.Sensors.IsComputerUsed)
            {
                await _haDiscovery.PublishBinarySensorDiscoveryAsync("network_available", "Network Available", "connectivity");

            }
            if (_options.Sensors.NetworkSensor)
            {
                await _haDiscovery.PublishBinarySensorDiscoveryAsync("inuse", "User Activity", "occupancy");
            }
        }

    }

}
