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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _publisher.PublishSystemData();
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(_options.TimerInterval, stoppingToken);
            }
        }
    }
}
