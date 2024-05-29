namespace Win2Mqtt.Infra.HardwareSensors
{
    public struct DriveStatus
    {
        public string DriveName { get; set; }
        public double TotalSize { get; set; }
        public double AvailableFreeSpace { get; set; }
        public double PercentFree { get; set; }
    }
    public static class Drives
    {
        public static IEnumerable<DriveStatus> GetDriveStatus()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                yield return new DriveStatus
                {
                    DriveName = drive.Name.Replace(":\\", ""),
                    TotalSize = drive.TotalSize,
                    AvailableFreeSpace = drive.AvailableFreeSpace,
                    PercentFree = Math.Round((double)drive.TotalFreeSpace / drive.TotalSize * 100, 0)
                };
            }
        }
    }

}