using System.Threading.Tasks;

namespace Win2Mqtt
{
    public interface IIncomingMessagesProcessor
    {
        Task ProcessMessageAsync(string subtopic, string message);
    }
}
