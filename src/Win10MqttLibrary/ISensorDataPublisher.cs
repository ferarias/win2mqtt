using System.Threading.Tasks;

namespace Win2Mqtt.Client.Mqtt
{
    public interface ISensorDataPublisher
    {
        Task PublishSystemDataAsync();
    }
}