using Microsoft.Extensions.DependencyInjection;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWin2MqttApplication(this IServiceCollection services) =>
            services
            .AddSingleton<ISensorFactory, SensorFactory>()
            .AddSingleton<IActionFactory, ActionFactory>()
            .AddTransient<IIncomingMessagesProcessor, IncomingMessagesProcessor>();
    }
}
