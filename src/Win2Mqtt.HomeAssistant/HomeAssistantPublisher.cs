using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantPublisher(
        IMqttPublisher mqttPublisher,
        ISensorValueFormatter sensorValueFormatter,
        IOptions<Win2MqttOptions> options,
        ILogger<HomeAssistantPublisher> logger)
        : IMessagePublisher
    {
        private static readonly string HABaseTopic = "homeassistant";


        private static readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private readonly object DeviceInfo = new
        {
            identifiers = new[] { options.Value.DeviceUniqueId },
            name = $"Win2MQTT - {options.Value.DeviceUniqueId} metrics",
            manufacturer = "Win2MQTT",
            model = "System Monitoring"
        };

        public async Task PublishOnlineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{options.Value.MqttBaseTopic}/status";
            await mqttPublisher.PublishAsync(statusTopic, "online", retain: true, cancellationToken);
            logger.LogDebug("Published HA online status in {Topic}", statusTopic);
        }

        public async Task PublishOfflineStatus(CancellationToken cancellationToken = default)
        {
            var statusTopic = $"{options.Value.MqttBaseTopic}/status";
            await mqttPublisher.PublishAsync(statusTopic, "offline", retain: true, cancellationToken: cancellationToken);
            logger.LogDebug("Published HA offline status for {Topic}", statusTopic);
        }

        public async Task PublishSensorValue(ISensorWrapper sensor, object? value, CancellationToken cancellationToken = default)
        {
            if (sensor.Metadata.SensorStateTopic == null)
            {
                logger.LogWarning("Sensor {Sensor} has no state topic", sensor.Metadata.Key);
                return;
            }
            try
            {
                await mqttPublisher.PublishAsync(sensor.Metadata.SensorStateTopic, sensorValueFormatter.Format(value), false, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to publish sensor {Sensor}", sensor.Metadata.Key);
            }
        }

        public async Task PublishSensorDiscoveryMessage(SensorMetadata metadata, CancellationToken cancellationToken = default)
        {
            if (metadata.SensorStateTopic == null)
            {
                logger.LogWarning("Sensor {Sensor} has no state topic", metadata.Key);
                return;
            }
            if (string.IsNullOrWhiteSpace(metadata.SensorUniqueId))
            {
                logger.LogWarning("Sensor {Sensor} has no unique ID", metadata.Key);
                return;
            }
            // Build the common payload dictionary
            var payloadDict = new Dictionary<string, object>
            {
                ["name"] = metadata.Name,
                ["state_topic"] = metadata.SensorStateTopic,
                ["unique_id"] = metadata.SensorUniqueId,
                ["availability_topic"] = $"{options.Value.MqttBaseTopic}/status",
                ["device"] = DeviceInfo
            };

            if (!string.IsNullOrWhiteSpace(metadata?.UnitOfMeasurement)) payloadDict["unit_of_measurement"] = metadata.UnitOfMeasurement;
            if (!string.IsNullOrWhiteSpace(metadata?.StateClass)) payloadDict["state_class"] = metadata.StateClass;
            if (!string.IsNullOrWhiteSpace(metadata?.DeviceClass)) payloadDict["device_class"] = metadata.DeviceClass;

            string? discoveryTopic;
            if (metadata?.IsBinary == true)
            {
                // Binary-specific fields
                payloadDict["payload_on"] = "1";
                payloadDict["payload_off"] = "0";

                discoveryTopic = $"{HABaseTopic}/binary_sensor/{metadata.SensorUniqueId}/config";
            }
            else
            {
                discoveryTopic = $"{HABaseTopic}/sensor/{metadata?.SensorUniqueId}/config";
            }



            await mqttPublisher.PublishAsync(
            discoveryTopic,
                    JsonSerializer.Serialize(payloadDict, jsonSerializerOptions),
                    retain: true,
                    cancellationToken: cancellationToken);

            logger.LogInformation("Published HA binary_sensor config for {sensor}", metadata?.Key);

        }
    }
}
