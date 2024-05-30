using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Win2Mqtt.Infra.HardwareSensors;
using Win2Mqtt.Options;
using Win2Mqtt.Sensors.HardwareSensors;

namespace Win2Mqtt.Client.Mqtt
{
    public class SensorDataPublisher(IMqttConnector mqttConnector, 
        IOptions<Win2MqttOptions> options, 
        ILogger<SensorDataPublisher> logger)
        : ISensorDataPublisher, IAsyncDisposable
    {
        private readonly IMqttConnector _mqtt = mqttConnector;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<SensorDataPublisher> _logger = logger;

        public async Task PublishSystemDataAsync()
        {
            if (!(_mqtt.Connected || await _mqtt.ConnectAsync()))
            {
                _logger.LogError("Could not connect to broker");
                return;
            }

            if (_options.Sensors.IsComputerUsed)
            { 
                await _mqtt.PublishMessageAsync("binary_sensor/inUse", UsingComputer.IsUsing() ? "on" : "off");
            }
            if (_options.Sensors.CpuSensor)
            {
                await _mqtt.PublishMessageAsync("cpuprocessortime", Processor.GetCpuProcessorTime());
            }
            if (_options.Sensors.FreeMemorySensor)
            {
                await _mqtt.PublishMessageAsync("freememory", Memory.GetFreeMemory());
            }
            if (_options.Sensors.BatterySensor)
            {
                await _mqtt.PublishMessageAsync("availablebattery", Battery.GetAvailableBattery());
            }
            if (_options.Sensors.NetworkSensor)
            {
                await _mqtt.PublishMessageAsync("networkavailable", NetworkStatus.IsNetworkAvailable() ? "on" : "off");
            }
            if (_options.Sensors.DiskSensor)
            {
                foreach (var drive in Drives.GetDriveStatus())
                {
                    await _mqtt.PublishMessageAsync("drive/" + drive.DriveName + "/totalsize", drive.TotalSize.ToString(CultureInfo.CurrentCulture));
                    await _mqtt.PublishMessageAsync("drive/" + drive.DriveName + "/availablefreespace", drive.AvailableFreeSpace.ToString(CultureInfo.CurrentCulture));
                    await _mqtt.PublishMessageAsync("drive/" + drive.DriveName + "/percentfree", drive.PercentFree.ToString(CultureInfo.CurrentCulture));
                }
            }
        }

        #region "Disposable pattern"

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_mqtt != null && _mqtt.Connected)
            {
                await _mqtt.DisconnectAsync();
            }
        }

        #endregion "Disposable pattern"

    }
}