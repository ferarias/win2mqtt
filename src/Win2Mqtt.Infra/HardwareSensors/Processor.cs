using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Win2Mqtt.Infra.HardwareSensors
{
    public static class Processor
    {
        public static double GetCpuProcessorTime()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(1000);
            return Math.Round(cpuCounter.NextValue(), 2);
        }

        public static TimeSpan GetIdleTime()
        {
            var lastInPut = new Lastinputinfo();
            lastInPut.CbSize = (uint)Marshal.SizeOf(lastInPut);
            NativeMethods.GetLastInputInfo(ref lastInPut);

            return TimeSpan.FromMilliseconds((uint)Environment.TickCount - lastInPut.DwTime);
        }

        private class NativeMethods
        {
            [DllImport("User32.dll")]
            public static extern bool GetLastInputInfo(ref Lastinputinfo plii);
        }

    }

    internal struct Lastinputinfo
    {
        public uint CbSize;
        public uint DwTime;
    }

}