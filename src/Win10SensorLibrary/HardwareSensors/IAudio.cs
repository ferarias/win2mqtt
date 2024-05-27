namespace Win2Mqtt.Sensors.HardwareSensors
{
    public interface IAudio
    {
        string GetVolume();
        bool IsMuted();
        void Mute(bool Enable);
        void Volume(int level);
        void ChangeOutputDevice(string DeviceFullname);
    }
}