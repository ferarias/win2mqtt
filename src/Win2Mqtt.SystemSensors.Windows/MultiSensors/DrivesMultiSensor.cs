namespace Win2Mqtt.SystemSensors.Windows.MultiSensors
{
    public class DrivesMultiSensor() : SystemMultiSensorBase
    {
        public override IEnumerable<string> ChildIdentifiers => GetDriveLetters();

        private static IEnumerable<string> GetDriveLetters() => DriveInfo.GetDrives()
            .Where(di => di.IsReady && di.DriveType != DriveType.Network)
            .Select(di => di.Name.Replace(":\\", ""));
    }

}
