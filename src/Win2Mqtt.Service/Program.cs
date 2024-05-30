using Win2Mqtt.Client.Mqtt;
using Win2Mqtt.Client;
using Win2Mqtt.Options;
using Win2Mqtt.Service;
using Win2Mqtt.Infra;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IMqttConnector, MqttConnector>();
builder.Services.AddTransient<ISensorDataPublisher, SensorDataPublisher>();
builder.Services.AddTransient<INotifier, ToastMessage>();
builder.Services
    .AddOptions<Win2MqttOptions>()
    .BindConfiguration("Win2Mqtt")
    .ValidateDataAnnotations(); ;

var host = builder.Build();
host.Run();
