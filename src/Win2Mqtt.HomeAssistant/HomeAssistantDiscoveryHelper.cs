namespace Win2Mqtt.HomeAssistant
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Win2Mqtt.SystemMetrics;

    public class HomeAssistantDiscoveryHelper
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static (string Topic, string Payload) GetSensorDiscoveryMessage(string baseTopic, string deviceId, ISensorWrapper sensor)
        {
            ArgumentNullException.ThrowIfNull(sensor.Metadata.SensorUniqueId, nameof(sensor.Metadata.SensorUniqueId));
            ArgumentNullException.ThrowIfNull(sensor.Metadata.SensorStateTopic, nameof(sensor.Metadata.SensorStateTopic));

            // e.g.: "homeassistant/sensor/win2mqtt_winsrv02_cpu_usage/config"
            var homeAssistantConfigTopic = $"{HomeAssistantTopics.BaseTopic}/{(sensor.Metadata.IsBinary ? "binary_sensor" : "sensor")}/{sensor.Metadata.SensorUniqueId}/config";

            // e.g.: "win2mqtt/winsrv02/status"
            string availTopic = $"{baseTopic}/status";

            // Extract device info once
            var deviceInfo = new
            {
                identifiers = new[] { deviceId },
                name = $"Win2MQTT - {deviceId} metrics",
                manufacturer = "Win2MQTT",
                model = "System Monitoring"
            };

            // Build the common payload dictionary
            var payloadDict = new Dictionary<string, object>
            {
                ["name"] = sensor.Metadata.Name,
                ["state_topic"] = sensor.Metadata.SensorStateTopic,
                ["unique_id"] = sensor.Metadata.SensorUniqueId ?? "Unknown",
                ["availability_topic"] = availTopic,
                ["device"] = deviceInfo
            };

            if(!string.IsNullOrWhiteSpace(sensor.Metadata?.UnitOfMeasurement)) payloadDict["unit_of_measurement"] = sensor.Metadata.UnitOfMeasurement;
            if (!string.IsNullOrWhiteSpace(sensor.Metadata?.StateClass)) payloadDict["state_class"] = sensor.Metadata.StateClass;
            if (!string.IsNullOrWhiteSpace(sensor.Metadata?.DeviceClass)) payloadDict["device_class"] = sensor.Metadata.DeviceClass;

            if (sensor.Metadata?.IsBinary == true)
            {
                // Binary-specific fields
                payloadDict["payload_on"] = "1";
                payloadDict["payload_off"] = "0";
            }

            return (homeAssistantConfigTopic, JsonSerializer.Serialize(payloadDict, jsonSerializerOptions));
        }
    }
}