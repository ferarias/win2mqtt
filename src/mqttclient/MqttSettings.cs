using System.Globalization;

namespace Win2Mqtt.Client
{
    public static class MqttSettings
    {
        public static string MqttServer
        {
            get => (string)Properties.Settings.Default["mqttserver"];
            set => Properties.Settings.Default["mqttserver"] = value;
        }
        public static string MqttUsername
        {
            get => (string)Properties.Settings.Default["mqttusername"];
            set => Properties.Settings.Default["mqttusername"] = value;
        }
        public static string MqttPassword
        {
            get => (string)Properties.Settings.Default["mqttpassword"];
            set => Properties.Settings.Default["mqttpassword"] = value;
        }
        public static string MqttTopic
        {
            get => (string)Properties.Settings.Default["mqtttopic"];
            set => Properties.Settings.Default["mqtttopic"] = value;
        }
        public static int MqttPort
        {
            get => (int)Properties.Settings.Default["mqttport"];
            set => Properties.Settings.Default["mqttport"] = value.ToString(CultureInfo.CurrentCulture);
        }
        internal static void Init()
        {
            Properties.Settings.Default.Upgrade();
        }
        public static string MqttTimerInterval
        {
            get => (string)Properties.Settings.Default["mqtttimerinterval"];
            set => Properties.Settings.Default["mqtttimerinterval"] = value.ToString(CultureInfo.CurrentCulture);
        }
        public static bool MinimizeToTray
        {
            get => (bool)Properties.Settings.Default[nameof(MinimizeToTray)];
            set => Properties.Settings.Default[nameof(MinimizeToTray)] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool CpuSensor
        {
            get => (bool)Properties.Settings.Default["Cpusensor"];
            set => Properties.Settings.Default["Cpusensor"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool FreeMemorySensor
        {
            get => (bool)Properties.Settings.Default["Freememorysensor"];
            set => Properties.Settings.Default["Freememorysensor"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool VolumeSensor
        {
            get => (bool)Properties.Settings.Default["Volumesensor"];
            set => Properties.Settings.Default["Volumesensor"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool IsComputerUsed
        {
            get => (bool)Properties.Settings.Default[nameof(IsComputerUsed)];
            set => Properties.Settings.Default[nameof(IsComputerUsed)] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool BatterySensor
        {
            get => (bool)Properties.Settings.Default[nameof(BatterySensor)];
            set => Properties.Settings.Default[nameof(BatterySensor)] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool DiskSensor
        {
            get => (bool)Properties.Settings.Default[nameof(DiskSensor)];
            set => Properties.Settings.Default[nameof(DiskSensor)] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool Monitor
        {
            get => (bool)Properties.Settings.Default["CmdMonitor"];
            set => Properties.Settings.Default["CmdMonitor"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool Toast
        {
            get => (bool)Properties.Settings.Default["CmdToast"];
            set => Properties.Settings.Default["CmdToast"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool App
        {
            get => (bool)Properties.Settings.Default["CmdApp"];
            set => Properties.Settings.Default["CmdApp"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool Hibernate
        {
            get => (bool)Properties.Settings.Default["CmdHibernate"];
            set => Properties.Settings.Default["CmdHibernate"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool Shutdown
        {
            get => (bool)Properties.Settings.Default["CmdShutdown"];
            set => Properties.Settings.Default["CmdShutdown"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool Reboot
        {
            get => (bool)Properties.Settings.Default["CmdReboot"];
            set => Properties.Settings.Default["CmdReboot"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        public static bool Suspend
        {
            get => (bool)Properties.Settings.Default["CmdSuspend"];
            set => Properties.Settings.Default["CmdSuspend"] = Convert.ToBoolean(value.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        }
        internal static void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
