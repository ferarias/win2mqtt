namespace Win2Mqtt.SystemActions
{
    public class SwitchMetadata
    {
        public required string Key { get; set; }

        public required string Name { get; set; }

        public required string UniqueId { get; set; }

        public required string StateTopic { get; set; }

        public required string CommandTopic { get; set; }
    }
}
