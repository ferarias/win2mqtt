namespace Win2Mqtt.HomeAssistant
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Win2Mqtt;
    using Win2Mqtt.Common;
    using Win2Mqtt.SystemMetrics;

    public class HomeAssistantDiscoveryHelper
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static (string HomeAssistantConfigTopic, string Payload) GetSensorDiscoveryPayload(
            string machineIdentifier,
            SensorMetadata meta)
        {
            // Common values
            // e.g.: "win2mqtt_winsrv02"
            var machineUniqueId = $"{Constants.Win2MqttTopic}_{machineIdentifier}";

            // e.g.: "win2mqtt_winsrv02_cpu_usage"
            var sensorUniqueId = $"{Constants.Win2MqttTopic}_{machineIdentifier}_{SanitizeHelpers.Sanitize(meta.Key)}";

            // e.g.: "homeassistant/sensor/win2mqtt_winsrv02_cpu_usage/config"
            var homeAssistantConfigTopic = $"{HomeAssistantTopics.BaseTopic}/{(meta.IsBinary ? "binary_sensor" : "sensor")}/{sensorUniqueId}/config";

            // e.g.: "win2mqtt/winsrv02"
            string mqttBaseTopic = $"{Constants.Win2MqttTopic}/{machineIdentifier}";

            // e.g.: "win2mqtt/winsrv02/win2mqtt_winsrv02_cpu_usage"
            string stateTopic = $"{mqttBaseTopic}/{sensorUniqueId}";

            // e.g.: "win2mqtt/winsrv02/status"
            string availTopic = $"{mqttBaseTopic}/status";

            // Extract device info once
            var deviceInfo = new
            {
                identifiers = new[] { machineUniqueId },
                name = $"Win2MQTT - {machineIdentifier}",
                manufacturer = "Win2MQTT",
                model = "System Monitoring"
            };

            // Build the common payload dictionary
            var payloadDict = new Dictionary<string, object>
            {
                ["name"] = meta.Name,
                ["state_topic"] = stateTopic,
                ["unique_id"] = sensorUniqueId,
                ["availability_topic"] = availTopic,
                ["device"] = deviceInfo
            };

            if(!string.IsNullOrWhiteSpace(meta?.UnitOfMeasurement)) payloadDict["unit_of_measurement"] = meta.UnitOfMeasurement;
            if (!string.IsNullOrWhiteSpace(meta?.StateClass)) payloadDict["state_class"] = meta.StateClass;
            if (!string.IsNullOrWhiteSpace(meta?.DeviceClass)) payloadDict["device_class"] = meta.DeviceClass;

            if (meta?.IsBinary == true)
            {
                // Binary-specific fields
                payloadDict["payload_on"] = "1";
                payloadDict["payload_off"] = "0";
            }

            return (homeAssistantConfigTopic, JsonSerializer.Serialize(payloadDict, jsonSerializerOptions));
        }
    }
}