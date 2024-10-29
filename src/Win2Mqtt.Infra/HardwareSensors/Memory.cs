using System.Diagnostics;

namespace Win2Mqtt.Infra.HardwareSensors
{
    public static class Memory
    {
        public static float GetFreeMemory()
        {
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }
    }
}