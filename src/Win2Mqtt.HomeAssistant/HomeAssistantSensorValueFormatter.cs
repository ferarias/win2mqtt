using System.Globalization;
using Win2Mqtt.SystemSensors;

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

        public T? Format<T>(string? value) where T : struct
        {
            if(string.IsNullOrEmpty(value))
                return default;

            return typeof(T) switch
            {
                Type t when t == typeof(bool) => (T)(object)(value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase)),
                Type t when t == typeof(DateTime) => (T)(object)DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                Type t when t == typeof(DateTimeOffset) => (T)(object)DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind),
                Type t when t == typeof(double) => (T)(object)double.Parse(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(float) => (T)(object)float.Parse(value, CultureInfo.InvariantCulture),
                Type t when t == typeof(int) => (T)(object)int.Parse(value, CultureInfo.InvariantCulture),
                _ => (T)(object)value
            };
        }
    }

}
