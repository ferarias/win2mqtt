using Microsoft.Extensions.Options;
using Win2Mqtt.Client.Mqtt;
using Win2Mqtt.Options;

namespace Win2Mqtt.Service
{
    public class Worker(
        ISensorDataCollector collector,
        IMqttConnector connector,
        IOptions<Win2MqttOptions> options,
        ILogger<Worker> logger) : BackgroundService
    {
        private readonly ISensorDataCollector _collector = collector;
        private readonly IMqttConnector _connector = connector;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<Worker> _logger = logger;

        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started");

            if (await _connector.ConnectAsync() && await _connector.SubscribeAsync())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await _semaphore.WaitAsync(stoppingToken);
                    try
                    {
                        var sensorsData = await _collector.CollectSystemDataAsync();
                        foreach (var sensorData in sensorsData)
                        {
                            await _connector.PublishMessageAsync(sensorData.Key, sensorData.Value);
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                    await Task.Delay(TimeSpan.FromSeconds(_options.TimerInterval), stoppingToken);
                }
            }
        }
    }
}
