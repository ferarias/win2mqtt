using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Win2Mqtt.Infra.HardwareSensors;
using Win2Mqtt.Options;

namespace Win2Mqtt.Client.Mqtt
{
    public class SensorDataCollector(
        IOptions<Win2MqttOptions> options, 
        ILogger<SensorDataCollector> logger)
        : ISensorDataCollector
    {
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<SensorDataCollector> _logger = logger;

        public Task<IDictionary<string, string>> CollectSystemDataAsync()
        {
            _logger.LogDebug("Collecting sensor data from system");

            var data = new Dictionary<string, string>();
            if (_options.Sensors.DiskSensor)
            {
                foreach (var drive in Drives.GetDriveStatus())
                {
                    var topic = "drive/" + drive.DriveName;
                    data.Add($"{topic}/totalsize", drive.TotalSize.ToString(CultureInfo.CurrentCulture));
                    data.Add($"{topic}/availablefreespace", drive.AvailableFreeSpace.ToString(CultureInfo.CurrentCulture));
                    data.Add($"{topic}/percentfree", drive.PercentFree.ToString(CultureInfo.CurrentCulture));
                }
            }
            if (_options.Sensors.FreeMemorySensor)
            {
                data.Add("freememory", string.Format("{0}MB", Memory.GetFreeMemory()));
            }
            if (_options.Sensors.NetworkSensor)
            {
                data.Add("networkavailable", NetworkStatus.IsNetworkAvailable() ? "1" : "0");
            }
            if (_options.Sensors.CpuSensor)
            {
                data.Add("cpuprocessortime", string.Format("{0}%", Processor.GetCpuProcessorTime()));
            }
            if (_options.Sensors.IsComputerUsed)
            { 
                data.Add("binary_sensor/inuse", Processor.GetIdleTime().TotalSeconds <= 30 ? "1" : "0");
            }
            return Task.FromResult<IDictionary<string, string>>(data);
        }
    }
}