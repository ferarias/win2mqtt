using Microsoft.Extensions.Options;
using Win2Mqtt.Client.Mqtt;
using Win2Mqtt.Options;

namespace Win2Mqtt.Service
{
    public class Worker(ISensorDataPublisher sensorDataPublisher, IOptions<Win2MqttOptions> options, ILogger<Worker> logger) : BackgroundService
    {
        private readonly ISensorDataPublisher _publisher = sensorDataPublisher;
        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<Worker> _logger = logger;

        private readonly static SemaphoreSlim _semaphore = new(1,1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Worker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await _semaphore.WaitAsync(stoppingToken);
                try
                {
                    await _publisher.PublishSystemDataAsync();
                }
                finally
                {
                    _semaphore.Release();
                }
                await Task.Delay(_options.TimerInterval, stoppingToken);
            }
        }
    }
}
