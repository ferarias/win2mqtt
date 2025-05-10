using Microsoft.Extensions.Logging;
using System.Globalization;

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
                return DriveInfo.GetDrives()
                    .Where(di => di.IsReady && di.DriveType != DriveType.Network)
                    .SelectMany(di =>
                    {
                        var driveName = di.Name.Replace(":\\", "");
                        return new[]
                        {
                            new KeyValuePair<string, string>($"drive/{driveName}/sizetotal", di.TotalSize.ToString(CultureInfo.InvariantCulture)),
                            new KeyValuePair<string, string>($"drive/{driveName}/sizefree", di.AvailableFreeSpace.ToString(CultureInfo.InvariantCulture)),
                            new KeyValuePair<string, string>($"drive/{driveName}/percentfree",
                            Math.Round((double)di.TotalFreeSpace / di.TotalSize * 100, 0).ToString(CultureInfo.InvariantCulture))
                        };
                    })
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error collecting disk data");
                return new Dictionary<string, string>();
            }
        }
    }

}