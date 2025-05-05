using CliWrap;
using Serilog;
using Win2Mqtt.Broker.MQTTNet;
using Win2Mqtt.Common;
using Win2Mqtt.Common.Options;
using Win2Mqtt.HomeAssistant;
using Win2Mqtt.Service;
using Win2Mqtt.System.Actions;
using Win2Mqtt.System.Metrics;

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

    Log.Information("Start service.");

    var builder = Host.CreateApplicationBuilder(args);
    builder.Services
        .AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

    builder.Services
        .AddOptions<Win2MqttOptions>()
        .BindConfiguration(Win2MqttOptions.Options)
        .ValidateDataAnnotations();

    builder.Services
        .AddWindowsService(options => options.ServiceName = $"{Constants.AppId} Service")
        .AddMqtt2NetBroker()
        .AddHomeAssistantDiscovery()
        .AddSystemMetrics()
        .AddSystemActions()
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
