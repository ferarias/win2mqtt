using Microsoft.Extensions.Options;
using Win2Mqtt.Options;

namespace Win2Mqtt.Service
{
    public partial class WindowsBackgroundService(
        IMqttConnector connector,
        ISensorDataCollector collector,
        IIncomingMessagesProcessor messagesProcessor,
        IOptions<Win2MqttOptions> options,
        ILogger<WindowsBackgroundService> logger) : BackgroundService
    {
        // MQTT connector
        private readonly IMqttConnector _connector = connector;

        // System information collector
        private readonly ISensorDataCollector _collector = collector;

        // Incoming messages processor
        private readonly IIncomingMessagesProcessor _processor = messagesProcessor;

        private readonly Win2MqttOptions _options = options.Value;
        private readonly ILogger<WindowsBackgroundService> _logger = logger;

        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started");
            try
            {
                // Connect to MQTT broker
                if (await _connector.ConnectAsync())
                {
                    // Subscribe to incoming messages
                    // Process them with IIncomingMessagesProcessor.ProcessMessageAsync()
                    if (await _connector.SubscribeAsync(_processor.ProcessMessageAsync))
                    {
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            // Allow only one thread collecting system information
                            await _semaphore.WaitAsync(stoppingToken);
                            try
                            {
                                // Collect system information
                                var sensorsData = await _collector.CollectSystemDataAsync();

                                // Publish collected data
                                foreach (var sensorData in sensorsData)
                                {
                                    await _connector.PublishMessageAsync(sensorData.Key, sensorData.Value);
                                }
                                _logger.LogDebug("{sensorCount} sensors published", sensorsData.Count);
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
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }
}
