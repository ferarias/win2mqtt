using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Win2Mqtt.Options
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWin2MqttOptions(this IServiceCollection services)
        {
            services
            .AddOptionsWithValidateOnStart<Win2MqttOptions>()
                .BindConfiguration(Win2MqttOptions.SectionName)
                .ValidateDataAnnotations();

            services
                .PostConfigure<Win2MqttOptions>(o =>
                {
                    o.Sensors = new Dictionary<string, SensorOptions>(o.Sensors, StringComparer.OrdinalIgnoreCase);
                    o.MultiSensors = new Dictionary<string, MultiSensorOptions>(o.MultiSensors, StringComparer.OrdinalIgnoreCase);
                    o.Listeners = new Dictionary<string, ListenerOptions>(o.Listeners, StringComparer.OrdinalIgnoreCase);
                });

            return services;
        }
    }
}
