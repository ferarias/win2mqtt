namespace Win2Mqtt.Infra.HomeAssistant
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using Win2Mqtt.Options;

    public class HomeAssistantDiscoveryHelper(
    IMqttConnector connector,
    IOptions<Win2MqttOptions> options,
    ILogger<HomeAssistantDiscoveryHelper> logger)
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new() { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
        private readonly IMqttConnector _connector = connector;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<HomeAssistantDiscoveryHelper> _logger = logger;
        private readonly string _mqttBaseTopic = $"win2mqtt/{options.Value.MqttTopic}";

        private string Sanitize(string value) =>
            Regex.Replace(value.ToLowerInvariant(), @"[^a-z0-9_]+", "_");

        private string DiscoveryTopic(string entityType, string id) =>
            $"homeassistant/{entityType}/{id}/config";

        private string SensorId(string sensorKey) =>
            $"win2mqtt_{Sanitize(_options.MqttTopic)}_{Sanitize(sensorKey)}";

        public async Task PublishSensorDiscoveryAsync(string sensorKey, string name, string? unit = null, string? deviceClass = null, string? stateClass = null)
        {
            var id = SensorId(sensorKey);
            var configTopic = DiscoveryTopic("sensor", id);
            var stateTopic = $"{_mqttBaseTopic}/{Sanitize(sensorKey)}";

            var payload = new
            {
                name,
                state_topic = stateTopic,
                unique_id = id,
                availability_topic = $"{_mqttBaseTopic}/status",
                device = new
                {
                    identifiers = new[] { $"win2mqtt_{_options.MqttTopic}" },
                    name = $"Win2MQTT - {_options.MqttTopic}",
                    manufacturer = "Win2MQTT",
                    model = "System Monitoring"
                },
                unit_of_measurement = unit,
                device_class = deviceClass,
                state_class = stateClass
            };

            await _connector.PublishToFullTopicAsync(configTopic, JsonSerializer.Serialize(payload, jsonSerializerOptions), retain: true);
            _logger.LogInformation("Published HA discovery config for {sensor}", sensorKey);
        }

        public async Task PublishBinarySensorDiscoveryAsync(string sensorKey, string name, string? deviceClass = null)
        {
            var id = SensorId(sensorKey);
            var configTopic = DiscoveryTopic("binary_sensor", id);
            var stateTopic = $"{_mqttBaseTopic}/{Sanitize(sensorKey)}";

            var payload = new
            {
                name,
                state_topic = stateTopic,
                unique_id = id,
                availability_topic = $"{_mqttBaseTopic}/status",
                payload_on = "1",
                payload_off = "0",
                device = new
                {
                    identifiers = new[] { $"win2mqtt_{_options.MqttTopic}" },
                    name = $"Win2MQTT - {_options.MqttTopic}",
                    manufacturer = "Win2MQTT",
                    model = "System Monitoring"
                },
                device_class = deviceClass
            };

            var json = JsonSerializer.Serialize(payload, jsonSerializerOptions);
            await _connector.PublishToFullTopicAsync(configTopic, json, retain: true);
            _logger.LogInformation("Published HA binary_sensor config for {sensor}", sensorKey);
        }

        public async Task UnpublishSensorDiscoveryAsync(string sensorKey, bool isBinary = false)
        {
            var id = SensorId(sensorKey);
            var topic = DiscoveryTopic(isBinary ? "binary_sensor" : "sensor", id);
            await _connector.PublishToFullTopicAsync(topic, "", retain: true);
            _logger.LogInformation("Unpublished HA discovery config for {sensor}", sensorKey);
        }
    }
}