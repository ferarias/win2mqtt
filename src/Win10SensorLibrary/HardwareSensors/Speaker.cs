using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;

namespace Win2Mqtt.Sensors.HardwareSensors
{
    public static class Speaker
    {
        public static List<string> GetSpeakers()
        {
            List<string> result = new List<string>();
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            foreach (InstalledVoice i in synthesizer.GetInstalledVoices(CultureInfo.CurrentCulture))
            {
                result.Add(i.VoiceInfo.Name);
            }
            return result;
        }
        public static void Speak(string Text, string Device)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoice(Device);
            synthesizer.Speak(Text);
        }
    }
}
