using System.Runtime.InteropServices;

namespace Win2Mqtt.Infra.SystemOperations
{

    public static partial class Monitor
    {
        [LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
        private static partial IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private const int HWND_BROADCAST = 0xffff;
        private const int WmSyscommand = 0x0112;
        private const int ScMonitorpower = 0xF170;
        private const int MonitorTurnOn = -1;
        private const int MonitorShutoff = 2;

        public static void Set(bool value) 
        { 
            if (value)
            {
                TurnOn();
            }
            else 
                TurnOff(); 
        }

        public static void TurnOn()
        {
            SendMessage(HWND_BROADCAST, WmSyscommand, (IntPtr)ScMonitorpower, (IntPtr)MonitorTurnOn);
        }

        public static void TurnOff()
        {
            SendMessage(HWND_BROADCAST, WmSyscommand, (IntPtr)ScMonitorpower, (IntPtr)MonitorShutoff);

        }
    }
}
