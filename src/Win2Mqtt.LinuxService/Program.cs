using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Win2Mqtt.Options;

AssemblyLoadContext.Default.Unloading += SigTermEventHandler;
Console.CancelKeyPress += CancelHandler;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();


try
{
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

void CancelHandler(object? sender, ConsoleCancelEventArgs e)
{
    Console.WriteLine("App was stopped by the user.");
}

void SigTermEventHandler(AssemblyLoadContext context)
{
    Console.WriteLine("Service was stopped.");
}
