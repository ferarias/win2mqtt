using CommunityToolkit.WinUI.Notifications;
using Win2Mqtt.Client;

namespace Win2Mqtt.Infra
{
    public class ToastMessage : INotifier
    {
        public void ShowText(IList<string> lines)
        {
            var t = new ToastContentBuilder();
            t.AddArgument("conversationId", Constants.AppId);
            foreach (var item in lines)
            {
                t.AddText(item);
            };
            t.Show();
        }
        public void ShowImage(IList<string> lines, string imageUrl)
        {
            var t = new ToastContentBuilder();
            t.AddArgument("conversationId", Constants.AppId);
            t.AddInlineImage(new Uri("file:///" + imageUrl));
            foreach (var item in lines)
            {
                t.AddText(item);
            };
            t.Show();

        }
    }
}
