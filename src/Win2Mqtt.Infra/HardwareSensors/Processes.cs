using System.Diagnostics;

namespace Win2Mqtt.Sensors.HardwareSensors
{
    public static class Processes
    {
        public static string IsRunning(string exename)
        {
            try
            {
                return Process.GetProcessesByName(exename).Length != 0 == true ? "1" : "0";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public static string Close(string exename)
        {
            try
            {
                foreach (var proc in Process.GetProcessesByName(exename))
                {
                    proc.Kill();
                }
                return "1";
            }
            catch
            {
                return "0";
            }
        }
    }
}