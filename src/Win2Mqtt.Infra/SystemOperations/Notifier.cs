using CommunityToolkit.WinUI.Notifications;

namespace Win2Mqtt.Infra.SystemOperations
{
    public class NotifierParameters
    {
        public required string[] Lines { get; set; }
        public string? Image { get; set; }
    }

    public static class Notifier
    {
        public static void Show(NotifierParameters parameters)
        {
            var builder = new ToastContentBuilder();
            builder.AddArgument("conversationId", Constants.AppId);
            foreach (var item in parameters.Lines)
            {
                builder.AddText(item);
            };
            if (!string.IsNullOrWhiteSpace(parameters.Image))
            {
                builder.AddInlineImage(new Uri("file:///" + parameters.Image));
            }
            builder.Show();
        }
    }
}
