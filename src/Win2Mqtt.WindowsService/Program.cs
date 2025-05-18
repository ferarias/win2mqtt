using Serilog;
using Win2Mqtt.Application;
using Win2Mqtt.Broker.MQTTNet;
using Win2Mqtt.HomeAssistant;
using Win2Mqtt.Options;
using Win2Mqtt.SystemMetrics;


#if WINDOWS
using Win2Mqtt;
using Win2Mqtt.SystemActions.Windows;
using Win2Mqtt.SystemMetrics.Windows;
#endif

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

#if WINDOWS
// Manage installation and uninstallation of the service (CliWrap)
if (await WindowsServiceInstaller.HandleServiceInstallationAsync(args))
    return;
#endif

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
        .AddHomeAssistant()
        .AddSystemSensors();
#if WINDOWS
    builder.Services
        .AddWindowsSpecificSensors()
        .AddWindowsSystemActions()
        .AddWindowsService(options => options.ServiceName = $"{Constants.AppId} Service");
#endif

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
