using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Win2Mqtt.Options;

namespace Win2Mqtt.SystemMetrics.Windows
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
                try
                {
                    foreach (var drive in Drives.GetDriveStatus())
                    {
                        var topic = "drive/" + drive.DriveName;
                        data.Add($"{topic}/sizetotal", drive.TotalSize.ToString(CultureInfo.InvariantCulture));
                        data.Add($"{topic}/sizefree", drive.AvailableFreeSpace.ToString(CultureInfo.InvariantCulture));
                        data.Add($"{topic}/percentfree", drive.PercentFree.ToString(CultureInfo.InvariantCulture));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error collecting disk sensor data");
                }
            }
            if (_options.Sensors.FreeMemorySensor)
            {
                try
                {
                    data.Add("freememory", Memory.GetFreeMemory().ToString(CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error collecting free memory sensor data");
                }
            }
            if (_options.Sensors.CpuSensor)
            {
                try
                {
                    data.Add("cpuprocessortime", Processor.GetProcessorTime().ToString(CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error collecting CPU sensor data");
                }
            }
            if (_options.Sensors.IsComputerUsed)
            {
                try
                {
                    data.Add("binary_sensor/inuse", (Processor.GetIdleTime().TotalSeconds <= 30).BooleanToMqttOneOrZero());
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error collecting computer usage sensor data");
                }
            }
            if (_options.Sensors.NetworkSensor)
            {
                try
                {
                    data.Add("binary_sensor/network_available", NetworkStatus.IsNetworkAvailable().BooleanToMqttOneOrZero());
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error collecting network sensor data");
                }
            }
            return Task.FromResult<IDictionary<string, string>>(data);
        }
    }
}