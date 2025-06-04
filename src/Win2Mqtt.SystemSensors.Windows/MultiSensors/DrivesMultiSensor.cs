namespace Win2Mqtt.SystemSensors.Windows.MultiSensors
{
    public class DrivesMultiSensor() : SystemMultiSensorBase
    {
        public override IEnumerable<string> ChildIdentifiers => 
            DriveInfo.GetDrives()
            .Where(di => di.IsReady && di.DriveType != DriveType.Network)
            .Select(di => di.Name.Replace(":\\", ""));
    }

}
