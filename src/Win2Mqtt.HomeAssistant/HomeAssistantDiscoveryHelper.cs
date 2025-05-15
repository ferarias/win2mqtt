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

        public static object GetHADeviceInfo(string deviceUniqueId)
        {
            return new
            {
                identifiers = new[] { deviceUniqueId },
                name = $"Win2MQTT - {deviceUniqueId} metrics",
                manufacturer = "Win2MQTT",
                model = "System Monitoring"
            };
        }

        public static (string Topic, string Payload) GetHADiscoveryMessage(string baseTopic, SensorMetadata metadata, object deviceInfo)
        {
            ArgumentNullException.ThrowIfNull(metadata, nameof(metadata));
            ArgumentNullException.ThrowIfNull(metadata.SensorStateTopic, nameof(metadata.SensorStateTopic));

            // Build the common payload dictionary
            var payloadDict = new Dictionary<string, object>
            {
                ["name"] = metadata.Name,
                ["state_topic"] = metadata.SensorStateTopic,
                ["unique_id"] = metadata.SensorUniqueId ?? "Unknown",
                ["availability_topic"] = $"{baseTopic}/status",
                ["device"] = deviceInfo
            };

            if(!string.IsNullOrWhiteSpace(metadata?.UnitOfMeasurement)) payloadDict["unit_of_measurement"] = metadata.UnitOfMeasurement;
            if (!string.IsNullOrWhiteSpace(metadata?.StateClass)) payloadDict["state_class"] = metadata.StateClass;
            if (!string.IsNullOrWhiteSpace(metadata?.DeviceClass)) payloadDict["device_class"] = metadata.DeviceClass;

            ArgumentNullException.ThrowIfNull(metadata?.SensorUniqueId, nameof(metadata.SensorUniqueId));
            string? discoveryTopic;
            if (metadata?.IsBinary == true)
            {
                // Binary-specific fields
                payloadDict["payload_on"] = "1";
                payloadDict["payload_off"] = "0";

                discoveryTopic = $"{HomeAssistantTopics.BaseTopic}/binary_sensor/{metadata.SensorUniqueId}/config";
            }
            else
            {
                discoveryTopic = $"{HomeAssistantTopics.BaseTopic}/sensor/{metadata?.SensorUniqueId}/config";
            }
            return (discoveryTopic, JsonSerializer.Serialize(payloadDict, jsonSerializerOptions));
        }
    }
}