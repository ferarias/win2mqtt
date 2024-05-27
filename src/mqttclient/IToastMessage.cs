using System.Collections.Generic;

namespace Win2Mqtt.Client
{
    public interface IToastMessage
    {
        void ShowText(IList<string> lines);
        void ShowImage(IList<string> lines, string imageUrl);
    }
}