using Serilog;
using Win2Mqtt.Application;
using Win2Mqtt.Broker.MQTTNet;
using Win2Mqtt.HomeAssistant;
using Win2Mqtt.Options;
using Win2Mqtt.SystemSensors;
using Win2Mqtt.SystemActions;
using Win2Mqtt;


#if WINDOWS
using Win2Mqtt.SystemSensors.Windows;
using Win2Mqtt.SystemActions.Windows;
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
#if WINDOWS
    var appDataConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Constants.AppId, Constants.UserAppSettingsFile);
    builder.Configuration.AddJsonFile(appDataConfigPath, optional: true);
#else
    var appDataConfigPath = Path.Combine("/etc/", Constants.AppId.ToLowerInvariant(), Constants.UserAppSettingsFile);
    builder.Configuration.AddJsonFile(appDataConfigPath, optional: true);
#endif

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
        .AddSystemActions()
        .AddSystemSensors();
#if WINDOWS
    builder.Services
        .AddWindowsSpecificSensors()
        .AddWindowsSpecificSystemActions()
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
