using Microsoft.Extensions.DependencyInjection;
using Samqtt.SystemActions;
using Samqtt.SystemSensors;

namespace Samqtt.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWin2MqttApplication(this IServiceCollection services) =>
            services
            .AddSingleton<ISystemSensorFactory, SystemSensorFactory>()
            .AddSingleton<ISystemActionFactory, SystemActionFactory>();
    }
}
