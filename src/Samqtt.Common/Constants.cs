namespace Samqtt
{
    public static class Constants
    {
        public const string AppId = "Samqtt";
        public const string ServiceName = "Samqtt Service";

        public const string SamqttTopic = "samqtt";

        public static readonly string UserAppSettingsFile = $"{AppId.ToLowerInvariant()}.appsettings.json";
    }
}