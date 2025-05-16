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
        .AddWin2MqttOptions()
        .AddWin2MqttApplication()
        .AddMqtt2NetBroker()
        .AddHomeAssistant();

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
