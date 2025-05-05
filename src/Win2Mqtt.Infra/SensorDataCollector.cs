using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Common.Options;
using Win2Mqtt.Common;

namespace Win2Mqtt.System.Metrics
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

            var data = new Dictionary<string, string>
            {
                { "timestamp", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture) }
            };
            if (_options.Sensors.DiskSensor)
            {
                foreach (var drive in Drives.GetDriveStatus())
                {
                    var topic = "drive/" + drive.DriveName;
                    data.Add($"{topic}/sizetotal", drive.TotalSize.ToString(CultureInfo.InvariantCulture));
                    data.Add($"{topic}/sizefree", drive.AvailableFreeSpace.ToString(CultureInfo.InvariantCulture));
                    data.Add($"{topic}/percentfree", drive.PercentFree.ToString(CultureInfo.InvariantCulture));
                }
            }
            if (_options.Sensors.FreeMemorySensor)
            {
                data.Add("freememory", Memory.GetFreeMemory().ToString(CultureInfo.InvariantCulture));
            }
            if (_options.Sensors.CpuSensor)
            {
                data.Add("cpuprocessortime", Processor.GetProcessorTime().ToString(CultureInfo.InvariantCulture));
            }
            if (_options.Sensors.IsComputerUsed)
            {
                data.Add("binary_sensor/inuse", (Processor.GetIdleTime().TotalSeconds <= 30).BooleanToMqttOneOrZero());
            }
            if (_options.Sensors.NetworkSensor)
            {
                data.Add("binary_sensor/network_available", NetworkStatus.IsNetworkAvailable().BooleanToMqttOneOrZero());
            }
            return Task.FromResult<IDictionary<string, string>>(data);
        }
    }
}