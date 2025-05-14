using System.Globalization;
using Win2Mqtt.SystemMetrics;

namespace Win2Mqtt.HomeAssistant
{
    public class HomeAssistantSensorValueFormatter : ISensorValueFormatter
    {
        public string Format<T>(T? value)
        {
            if (value is null) return string.Empty;

            return value switch
            {
                bool b => b ? "1" : "0",
                DateTime dt => dt.ToUniversalTime().ToString("O"),
                DateTimeOffset dto => dto.ToUniversalTime().ToString("O"),
                double d => d.ToString("0.##", CultureInfo.InvariantCulture),
                float f => f.ToString("0.##", CultureInfo.InvariantCulture),
                _ => value.ToString() ?? string.Empty
            };
        }
    }

}
