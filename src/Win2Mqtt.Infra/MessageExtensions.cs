namespace Win2Mqtt.Client.Mqtt
{
    internal static class MessageExtensions
    {
        public static int? AsNullableInt(this string message) => int.TryParse(message, out var delay) ? delay : null;
        public static int AsInt(this string message, int defaultValue = default) => int.TryParse(message, out var delay) ? delay : defaultValue;
    }
}