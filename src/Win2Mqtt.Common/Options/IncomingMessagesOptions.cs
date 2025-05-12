namespace Win2Mqtt.Options
{
    public class IncomingMessagesOptions
    {
        public bool Hibernate { get; set; }
        public bool Shutdown { get; set; }
        public bool Reboot { get; set; }
        public bool Suspend { get; set; }

        public bool StartProcess { get; set; }
        public bool GetProcesses { get; set; }
        public bool SendNotification { get; set; }
    }
}