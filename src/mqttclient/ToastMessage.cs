using CommunityToolkit.WinUI.Notifications;

namespace Win2Mqtt.Client
{
    public class ToastMessage : IToastMessage
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
