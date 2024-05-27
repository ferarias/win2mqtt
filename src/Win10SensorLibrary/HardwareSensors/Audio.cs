using System;
using System.Collections.Generic;
using System.Globalization;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Win2Mqtt.Sensors.HardwareSensors
{
    public class Audio : IAudio
    {


        public void Mute(bool Enable)
        {
            try
            {
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                defaultPlaybackDevice.Mute(Enable);

            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool IsMuted()
        {
            try
            {
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                return defaultPlaybackDevice.IsMuted;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void Volume(int level)
        {
            try
            {
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                defaultPlaybackDevice.Volume = Convert.ToDouble(level);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string GetVolume()
        {
            try
            {
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                return defaultPlaybackDevice.Volume + "%";
            }
            catch (Exception)
            {
                throw;
            }

        }
        static public List<string> GetAudioDevices()
        {
            CoreAudioController cac = new CoreAudioController();
            List<string> tmp = new List<string>();

            foreach (CoreAudioDevice de in cac.GetPlaybackDevices())
            {
                tmp.Add(de.FullName);
            }

            return tmp;

        }
        public void ChangeOutputDevice(string DeviceFullname)
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            CoreAudioController cac = new CoreAudioController();
            foreach (CoreAudioDevice de in cac.GetPlaybackDevices())
            {
                if (de.FullName.Equals(DeviceFullname, StringComparison.CurrentCultureIgnoreCase))
                {
                    defaultPlaybackDevice = de;
                }

            }
        }
    }
}