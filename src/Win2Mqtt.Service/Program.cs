using Serilog;
using Win2Mqtt;
using Win2Mqtt.Infra;
using Win2Mqtt.Options;
using Win2Mqtt.Service;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Services.AddWindowsService(options => options.ServiceName = $"{Constants.AppId} Service");

    builder.Services.AddSingleton<IMqttConnector, MqttConnector>();
    builder.Services.AddTransient<ISensorDataCollector, SensorDataCollector>();
    builder.Services.AddTransient<IIncomingMessagesProcessor, IncomingMessagesProcessor>();
    builder.Services.AddSerilog();
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