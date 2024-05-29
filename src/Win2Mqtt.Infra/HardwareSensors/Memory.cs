using System.Diagnostics;

namespace Win2Mqtt.Sensors.HardwareSensors
{
    public static class Memory
    {
        public static string GetFreeMemory()
        {
            try
            {
                var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                return ramCounter.NextValue() + "MB";
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}