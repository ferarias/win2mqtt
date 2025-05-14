using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantDiscoveryPublisher(
        ISensorFactory sensorFactory,
        IHomeAssistantDiscoveryHelper homeAssistantDiscoveryHelper)
        : IHomeAssistantDiscoveryPublisher
    {
        private readonly IEnumerable<ISensorWrapper> _sensors = sensorFactory.GetEnabledSensors();


        public async Task PublishSensorsDiscoveryAsync(CancellationToken cancellationToken = default)
        {
            foreach (var sensor in _sensors)
            {
                var meta = sensor.Metadata;
                if (meta.IsBinary)
                {
                    await homeAssistantDiscoveryHelper.PublishBinarySensorDiscoveryAsync(
                        meta.Key, meta.Name, meta.DeviceClass, cancellationToken);
                }
                else
                {
                    await homeAssistantDiscoveryHelper.PublishSensorDiscoveryAsync(
                        meta.Key, meta.Name, meta.UnitOfMeasurement, meta.DeviceClass, meta.StateClass, cancellationToken);
                }
                await homeAssistantDiscoveryHelper.PublishSensorDiscoveryAsync("", "", "", null, "", cancellationToken);
            }
        }
    }
}
