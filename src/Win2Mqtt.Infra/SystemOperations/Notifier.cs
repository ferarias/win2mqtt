
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
            //TODO
            throw new NotImplementedException();
        }
    }
}
