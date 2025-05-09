using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class Processes
    {
        public static bool IsRunning(string exename)
        {
            try
            {
                return Process.GetProcessesByName(exename)?.Length != 0 == true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool Close(string exename)
        {
            try
            {
                foreach (var proc in Process.GetProcessesByName(exename))
                {
                    proc.Kill();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
