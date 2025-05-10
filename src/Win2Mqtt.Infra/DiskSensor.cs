using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemMetrics.Windows.WindowsSensors;

namespace Win2Mqtt.SystemMetrics.Windows
{
    public class DiskSensor : ISensor
    {
        private readonly ILogger<DiskSensor> _logger;

        public DiskSensor(ILogger<DiskSensor> logger)
        {
            _logger = logger;
        }

        public IDictionary<string, string> Collect()
        {
            try
            {
                var data = new Dictionary<string, string>();
                foreach (var drive in Drives.GetDriveStatus())
                {
                    var topic = "drive/" + drive.DriveName;
                    data.Add($"{topic}/sizetotal", drive.TotalSize.ToString(CultureInfo.InvariantCulture));
                    data.Add($"{topic}/sizefree", drive.AvailableFreeSpace.ToString(CultureInfo.InvariantCulture));
                    data.Add($"{topic}/percentfree", drive.PercentFree.ToString(CultureInfo.InvariantCulture));
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting disk data");
                return new Dictionary<string, string>();
            }
        }
    }

}