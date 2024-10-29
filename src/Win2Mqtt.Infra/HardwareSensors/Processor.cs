using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Win2Mqtt.Infra.HardwareSensors
{
    public static class Processor
    {
        public static double GetProcessorTime()
        {
            var perfCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            perfCounter.NextValue();
            Thread.Sleep(1000);
            return Math.Round(perfCounter.NextValue(), 2);
        }

        public static TimeSpan GetIdleTime()
        {
            var lastInput = new Lastinputinfo();
            lastInput.CbSize = (uint)Marshal.SizeOf(lastInput);
            NativeMethods.GetLastInputInfo(ref lastInput);

            return TimeSpan.FromMilliseconds((uint)Environment.TickCount - lastInput.DwTime);
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