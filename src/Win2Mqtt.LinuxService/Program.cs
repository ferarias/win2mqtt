using Serilog;
using Win2Mqtt.Application;
using Win2Mqtt.Broker.MQTTNet;
using Win2Mqtt.HomeAssistant;
using Win2Mqtt.Options;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Start application.");

    var builder = Host.CreateApplicationBuilder(args);
    builder.Services
        .AddHostedService<Win2MqttBackgroundService>();

    builder.Services
        .AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

    builder.Services
    .AddOptionsWithValidateOnStart<Win2MqttOptions>()
        .BindConfiguration(Win2MqttOptions.SectionName)
        .ValidateDataAnnotations();

    builder.Services
        .PostConfigure<Win2MqttOptions>(o =>
        {
            o.Sensors = new Dictionary<string, SensorOptions>(o.Sensors, StringComparer.OrdinalIgnoreCase);
            o.MultiSensors = new Dictionary<string, MultiSensorOptions>(o.MultiSensors, StringComparer.OrdinalIgnoreCase);
            o.Listeners = new Dictionary<string, ListenerOptions>(o.Listeners, StringComparer.OrdinalIgnoreCase);
        });

    builder.Services
        .AddMqtt2NetBroker()
        .AddHomeAssistant()
        .AddSingleton<Win2MqttService>();

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, ex.Message);
}
finally
{
    await Log.CloseAndFlushAsync();
}
