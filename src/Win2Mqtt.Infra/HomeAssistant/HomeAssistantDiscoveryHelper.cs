namespace Win2Mqtt.Infra.HomeAssistant
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Win2Mqtt.Common;
    using Win2Mqtt.Common.Options;

    public class HomeAssistantDiscoveryHelper(
        IMqttPublisher connector,
        IOptions<Win2MqttOptions> options,
        ILogger<HomeAssistantDiscoveryHelper> logger) : IHomeAssistantDiscoveryHelper
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private readonly IMqttPublisher _connector = connector;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<HomeAssistantDiscoveryHelper> _logger = logger;
        private readonly string _mqttBaseTopic = $"{Constants.ServiceBaseTopic}/{options.Value.MachineIdentifier}";

        private static string GetDiscoveryTopic(string entityType, string id) => $"{HomeAssistantTopics.BaseTopic}/{entityType}/{id}/config";

        private string GetSensorId(string sensorKey) => $"{Constants.ServiceBaseTopic}_{SanitizeHelpers.Sanitize(_options.MachineIdentifier)}_{SanitizeHelpers.Sanitize(sensorKey)}";

        public async Task PublishSensorDiscoveryAsync(string sensorKey, string name, string? unit = null, string? deviceClass = null, string? stateClass = null, CancellationToken cancellationToken = default)
        {
            var sensorId = GetSensorId(sensorKey); // e.g.: "win2mqtt_winsrv02_cpu_usage"
            var configTopic = GetDiscoveryTopic("sensor", sensorId); // e.g.: "homeassistant/sensor/win2mqtt_winsrv02_cpu_usage/config"
            var stateTopic = $"{_mqttBaseTopic}/{SanitizeHelpers.Sanitize(sensorKey)}"; // e.g.: "win2mqtt/winsrv02/cpu_usage"

            var payload = new
            {
                name,
                state_topic = stateTopic,
                unique_id = sensorId,
                availability_topic = $"{_mqttBaseTopic}/status",
                device = new
                {
                    identifiers = new[] { $"{Constants.ServiceBaseTopic}_{_options.MachineIdentifier}" },
                    name = $"Win2MQTT - {_options.MachineIdentifier}",
                    manufacturer = "Win2MQTT",
                    model = "System Monitoring"
                },
                unit_of_measurement = unit,
                device_class = deviceClass,
                state_class = stateClass
            };

            await _connector.PublishAsync(configTopic, JsonSerializer.Serialize(payload, jsonSerializerOptions), retain: true, cancellationToken: cancellationToken);
            _logger.LogInformation("Published HA discovery config for {sensor}", sensorKey);
        }

        public async Task PublishBinarySensorDiscoveryAsync(string sensorKey, string name, string? deviceClass = null, CancellationToken cancellationToken = default)
        {
            var sensorId = GetSensorId(sensorKey);
            var configTopic = GetDiscoveryTopic("binary_sensor", sensorId);
            var stateTopic = $"{_mqttBaseTopic}/{SanitizeHelpers.Sanitize(sensorKey)}";

            var payload = new
            {
                name,
                state_topic = stateTopic,
                unique_id = sensorId,
                availability_topic = $"{_mqttBaseTopic}/status",
                payload_on = "1",
                payload_off = "0",
                device = new
                {
                    identifiers = new[] { $"{Constants.ServiceBaseTopic}_{_options.MachineIdentifier}" },
                    name = $"Win2MQTT - {_options.MachineIdentifier}",
                    manufacturer = "Win2MQTT",
                    model = "System Monitoring"
                },
                device_class = deviceClass
            };

            var json = JsonSerializer.Serialize(payload, jsonSerializerOptions);
            await _connector.PublishAsync(configTopic, json, retain: true, cancellationToken: cancellationToken);
            _logger.LogInformation("Published HA binary_sensor config for {sensor}", sensorKey);
        }

        public async Task UnpublishSensorDiscoveryAsync(string sensorKey, bool isBinary = false, CancellationToken cancellationToken = default)
        {
            var sensorId = GetSensorId(sensorKey);
            var topic = GetDiscoveryTopic(isBinary ? "binary_sensor" : "sensor", sensorId);
            await _connector.PublishAsync(topic, "", retain: true, cancellationToken: cancellationToken);
            _logger.LogInformation("Unpublished HA discovery config for {sensor}", sensorKey);
        }
    }
}