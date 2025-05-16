using CliWrap;
using Serilog;
using Win2Mqtt;
using Win2Mqtt.Application;
using Win2Mqtt.Broker.MQTTNet;
using Win2Mqtt.HomeAssistant;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions.Windows;
using Win2Mqtt.SystemMetrics.Windows;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    // Manage installation and uninstallation of the service (CliWrap)
    var argsList = args.Select(a => a.ToLowerInvariant()).ToList();
    if (argsList.Contains("/install"))
    {
        Log.Information("Install service.");
        await Cli.Wrap("sc")
            .WithArguments(new[] { "create", Constants.ServiceName, $"binPath=\"{Environment.ProcessPath}\"", "start=auto" })
            .ExecuteAsync();
        return;
    }
    else if (argsList.Contains("/uninstall"))
    {
        Log.Information("Uninstall service.");
        await Cli.Wrap("sc")
            .WithArguments(new[] { "stop", Constants.ServiceName })
            .ExecuteAsync();

        await Cli.Wrap("sc")
            .WithArguments(new[] { "delete", Constants.ServiceName })
            .ExecuteAsync();
        return;
    }

    Log.Information("Start application.");

    var builder = Host.CreateApplicationBuilder(args);
    builder.Services
        .AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

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
        .AddWindowsService(options => options.ServiceName = $"{Constants.AppId} Service")
        .AddMqtt2NetBroker()
        .AddHomeAssistant()
        .AddWindowsSystemMetrics()
        .AddWindowsSystemActions()
        .AddSingleton<Win2MqttService>()
        .AddHostedService<Win2MqttBackgroundService>();

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
