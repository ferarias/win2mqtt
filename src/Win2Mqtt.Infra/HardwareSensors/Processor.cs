using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Win2Mqtt.Infra.HardwareSensors
{
    public static class Processor
    {
        private static PerformanceCounter? _cpuCounter;
        private static readonly object _lock = new object();

        static Processor()
        {
            // Initialize the performance counter only once
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public static double GetProcessorTime()
        {
            if (_cpuCounter == null)
                throw new InvalidOperationException("CPU performance counter is not initialized.");

            // First call to NextValue() is discarded to avoid inaccurate reading
            _cpuCounter.NextValue();
            Thread.Sleep(1000);  // Sleep for 1 second to get accurate reading
            return Math.Round(_cpuCounter.NextValue(), 2); // Return CPU usage as percentage
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