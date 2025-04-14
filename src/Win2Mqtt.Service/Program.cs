using CliWrap;
using Serilog;
using Win2Mqtt;
using Win2Mqtt.Infra;
using Win2Mqtt.Infra.HomeAssistant;
using Win2Mqtt.Options;
using Win2Mqtt.Service;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

if (args is { Length: 1 })
{
    try
    {
        string executablePath =
            Path.Combine(AppContext.BaseDirectory, "Win2MQTT.Service.exe");

        if (args[0] is "/Install")
        {
            await Cli.Wrap("sc")
                .WithArguments(new[] { "create", Constants.ServiceName, $"binPath={executablePath}", "start=auto" })
                .ExecuteAsync();
        }
        else if (args[0] is "/Uninstall")
        {
            await Cli.Wrap("sc")
                .WithArguments(new[] { "stop", Constants.ServiceName })
                .ExecuteAsync();

            await Cli.Wrap("sc")
                .WithArguments(new[] { "delete", Constants.ServiceName })
                .ExecuteAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }

    return;
}

try
{
    Log.Information("Getting the motors running...");

    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddWindowsService(options => options.ServiceName = $"{Constants.AppId} Service");

    builder.Services.AddSingleton<IMqttConnector, MqttConnector>();
    builder.Services.AddTransient<ISensorDataCollector, SensorDataCollector>();
    builder.Services.AddTransient<IIncomingMessagesProcessor, IncomingMessagesProcessor>();
    builder.Services.AddSingleton<Win2Mqtt.Infra.HomeAssistant.HomeAssistantDiscoveryHelper>();
    builder.Services.AddSingleton<HomeAssistantDiscoveryPublisher>();
    builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());
    builder.Services
        .AddOptions<Win2MqttOptions>()
        .BindConfiguration(Win2MqttOptions.Options)
        .ValidateDataAnnotations(); ;

    builder.Services.AddHostedService<WindowsBackgroundService>();
    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}