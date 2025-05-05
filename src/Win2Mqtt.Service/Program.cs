using CliWrap;
using MQTTnet;
using Serilog;
using Win2Mqtt.Common;
using Win2Mqtt.Common.Options;
using Win2Mqtt.Infra;
using Win2Mqtt.Infra.HomeAssistant;
using Win2Mqtt.Service;

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
        .AddWindowsService(options => options.ServiceName = $"{Constants.AppId} Service");

    builder.Services
        .AddSerilog((services, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext());

    builder.Services
        .AddSingleton<IMqttClient>(_ =>
        {
            var factory = new MqttClientFactory();
            return factory.CreateMqttClient();
        })
        .AddSingleton<IMqttConnectionManager, MqttConnector>()
        .AddSingleton<IMqttPublisher, MqttPublisher>()
        .AddSingleton<IMqttSubscriber, MqttSubscriber>()
        .AddTransient<ISensorDataCollector, SensorDataCollector>()
        .AddTransient<IIncomingMessagesProcessor, IncomingMessagesProcessor>()
        .AddSingleton<IHomeAssistantDiscoveryHelper, HomeAssistantDiscoveryHelper>()
        .AddSingleton<IHomeAssistantDiscoveryPublisher, HomeAssistantDiscoveryPublisher>()
        .AddOptions<Win2MqttOptions>()
            .BindConfiguration(Win2MqttOptions.Options)
            .ValidateDataAnnotations();

    builder.Services
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
