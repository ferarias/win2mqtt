using Win2Mqtt.Client.Mqtt;
using Win2Mqtt.Options;
using Win2Mqtt.Service;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddHostedService<Worker>();
    builder.Services.AddSingleton<IMqttConnector, MqttConnector>();
    builder.Services.AddTransient<ISensorDataCollector, SensorDataCollector>();
    builder.Services.AddSerilog();
    builder.Services
        .AddOptions<Win2MqttOptions>()
        .BindConfiguration("Win2Mqtt")
        .ValidateDataAnnotations(); ;

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