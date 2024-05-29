using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Win2Mqtt.Infra.HardwareSensors;
using Win2Mqtt.Options;
using Win2Mqtt.Sensors.HardwareSensors;

namespace Win2Mqtt.Client.Mqtt
{
    public class SensorDataPublisher(IMqttConnector mqttConnector, IOptions<Win2MqttOptions> options, ILogger<SensorDataPublisher> logger) : ISensorDataPublisher
    {
        private readonly IMqttConnector _mqtt = mqttConnector;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<SensorDataPublisher> _logger = logger;

        public async void PublishSystemData()
        {
            if (!_mqtt.Connected)
            {
                var connected = await _mqtt.ConnectAsync();
                if (!connected)
                {
                    _logger.LogError("Could not connect to broker");
                    return;
                }
            }

            List<Task> task = [];
            if (_mqtt.Connected == true)
            {
                if (_options.Sensors.IsComputerUsed)
                {
                    task.Add(Task.Run(() => _mqtt.PublishMessageAsync("binary_sensor/inUse", UsingComputer.IsUsing() ? "on" : "off")));
                }
                if (_options.Sensors.CpuSensor)
                {
                    task.Add(Task.Run(() => _mqtt.PublishMessageAsync("cpuprocessortime", Processor.GetCpuProcessorTime())));
                }
                if (_options.Sensors.FreeMemorySensor)
                {
                    task.Add(Task.Run(() => _mqtt.PublishMessageAsync("freememory", Memory.GetFreeMemory())));
                }
                if (_options.Sensors.BatterySensor)
                {
                    task.Add(Task.Run(() => _mqtt.PublishMessageAsync("availablebattery", Battery.GetAvailableBattery())));
                }
                if (_options.Sensors.DiskSensor)
                {
                    foreach (var drive in Drives.GetDriveStatus())
                    {
                        task.Add(Task.Run(() => _mqtt.PublishMessageAsync("drive/" + drive.DriveName + "/totalsize", drive.TotalSize.ToString(CultureInfo.CurrentCulture))));
                        task.Add(Task.Run(() => _mqtt.PublishMessageAsync("drive/" + drive.DriveName + "/availablefreespace", drive.AvailableFreeSpace.ToString(CultureInfo.CurrentCulture))));
                        task.Add(Task.Run(() => _mqtt.PublishMessageAsync("drive/" + drive.DriveName + "/percentfree", drive.PercentFree.ToString(CultureInfo.CurrentCulture))));
                    }
                }
                if (_options.Sensors.NetworkSensor)
                {
                    task.Add(Task.Run(() => _mqtt.PublishMessageAsync("networkavailable", NetworkStatus.IsNetworkAvailable() ? "on" : "off")));
                }
            }
            await Task.WhenAll(task).ConfigureAwait(false);
        }       
    }
}