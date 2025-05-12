namespace Win2Mqtt.Options
{
    public class SensorsOptions
    {
        public bool TimeSensor { get; set; }
        public bool CpuSensor { get; set; }
        public bool DiskSensor { get; set; }
        public bool MemorySensor { get; set; }
        public bool NetworkSensor { get; set; }
        public bool ComputerInUseSensor { get; set; }
    }
}
