namespace Win2Mqtt.Common
{
    public static class MessageExtensions
    {
        public static string BooleanToMqttOneOrZero(this bool? value) => value == true ? "1" : "0";
        public static string BooleanToMqttOneOrZero(this bool value) => value ? "1" : "0";
        public static string BooleanToMqttOnOff(this bool? value) => value == true ? "on" : "off";
        public static string BooleanToMqttOnOff(this bool value) => value ? "on" : "off";
        public static string BooleanToMqttYesNo(this bool? value) => value == true ? "yes" : "no";
        public static string BooleanToMqttYesNo(this bool value) => value ? "yes" : "no";

        public static int? MqttAsNullableInt(this string message) => int.TryParse(message, out var delay) ? delay : null;
        public static int MqttAsInt(this string message, int defaultValue = default) => int.TryParse(message, out var delay) ? delay : defaultValue;

        public static bool? MqttAsBoolean(this string value) => value switch
        {
            "1" => true,
            "0" => false,
            _ => null,
        };
        public static bool? MqttAsOnOff(this string value) => value switch
        {
            "on" => true,
            "off" => false,
            _ => null,
        };
        public static bool? MqttAsYesNo(this string value) => value switch
        {
            "yes" => true,
            "no" => false,
            _ => null,
        };

        public static bool? MqttAsAnyBoolean(this string value)
        {
            if (value.MqttAsBoolean() == true || value.MqttAsOnOff() == true || value.MqttAsYesNo() == true) return true;
            if (value.MqttAsBoolean() == false || value.MqttAsOnOff() == false || value.MqttAsYesNo() == false) return false;
            return null;
        }
    }
}