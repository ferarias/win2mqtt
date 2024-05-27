namespace Win2Mqtt.Client.Mqtt
{
    public interface IMqtt
    {
        string GMqtttopic { get; set; }

        bool IsConnected { get; }

        bool Connect(string hostname, int portNumber, string username, string password);
        
        void Disconnect();
        
        string FullTopic(string topic);
        
        void Publish(string topic, string message, bool retain = false);
        
        void PublishBytes(string topic, byte[] bytes);
        
        void PublishDiscovery(string topic, SensorType sensorType);
    }
}