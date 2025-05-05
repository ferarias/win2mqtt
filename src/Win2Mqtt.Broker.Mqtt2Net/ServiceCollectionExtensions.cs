using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using Win2Mqtt.Common;

namespace Win2Mqtt.Broker.MQTTNet
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMqtt2NetBroker(this IServiceCollection services) => 
            services
            .AddSingleton(_ =>
            {
                var factory = new MqttClientFactory();
                return factory.CreateMqttClient();
            })
            .AddSingleton<IMqttConnectionManager, MqttConnector>()
            .AddSingleton<IMqttPublisher, MqttPublisher>()
            .AddSingleton<IMqttSubscriber, MqttSubscriber>();
    }
}
