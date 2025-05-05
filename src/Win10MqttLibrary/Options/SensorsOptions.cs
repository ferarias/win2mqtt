namespace Win2Mqtt.Common.Options
{
    public class SensorsOptions
    {
        public bool CpuSensor { get; set; }
        public bool FreeMemorySensor { get; set; }
        public bool IsComputerUsed { get; set; }
        public bool DiskSensor { get; set; }
        public bool NetworkSensor { get; set; }

        public bool Hibernate { get; set; }
        public bool Shutdown { get; set; }
        public bool Reboot { get; set; }
        public bool Suspend { get; set; }

        public bool Monitor { get; set; }
        public bool App { get; set; }
    }
}
