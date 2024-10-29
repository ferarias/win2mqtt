using Microsoft.Win32;

namespace Win2Mqtt.Infra
{
    public static class WindowsRegistry
    {
        private const string StartupRegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static void EnableAtStartup(string executablePath)
        {
            var registryKey = Registry.CurrentUser.OpenSubKey(StartupRegistryKey, true);
            registryKey?.SetValue(Constants.AppId, executablePath);
        }
        public static void DisableAtStartup()
        {
            var registryKey = Registry.CurrentUser.OpenSubKey(StartupRegistryKey, true);
            registryKey?.DeleteValue(Constants.AppId, false);
        }
    }
}
