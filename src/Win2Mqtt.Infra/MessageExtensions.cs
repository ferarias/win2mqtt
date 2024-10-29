namespace Win2Mqtt.Client.Mqtt
{
    internal static class MessageExtensions
    {
        public static int? AsNullableInt(this string message) => int.TryParse(message, out var delay) ? delay : null;
        public static int AsInt(this string message, int defaultValue = default) => int.TryParse(message, out var delay) ? delay : defaultValue;

        public static string ToMqttBoolean(this bool value) => value ? "1" : "0";
        public static string ToMqttOnOff(this bool value) => value ? "on" : "off";
        public static string ToMqttYesNo(this bool value) => value ? "yes" : "no";

        public static bool? FromMqttBoolean(this string value) => value switch
        {
            "1" => true,
            "0" => false,
            _ => null,
        };
        public static bool? FromMqttOnOff(this string value) => value switch
        {
            "on" => true,
            "off" => false,
            _ => null,
        };
        public static bool? FromMqttYesNo(this string value) => value switch
        {
            "yes" => true,
            "no" => false,
            _ => null,
        };

        public static bool? FromMqttAnyBoolean(this string value)
        {
            if(value.FromMqttBoolean() == true || value.FromMqttOnOff() == true || value.FromMqttYesNo() == true) return true;
            if (value.FromMqttBoolean() == false || value.FromMqttOnOff() == false || value.FromMqttYesNo() == false) return false;
            return null;
        }
    }
}