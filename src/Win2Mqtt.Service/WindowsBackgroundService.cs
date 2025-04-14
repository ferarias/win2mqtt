using Microsoft.Extensions.Options;
using Win2Mqtt.Infra.HomeAssistant;
using Win2Mqtt.Options;

namespace Win2Mqtt.Service
{
    public class WindowsBackgroundService : BackgroundService
    {
        // MQTT connector
        private readonly IMqttConnector _connector;

        // System information collector
        private readonly ISensorDataCollector _collector;

        // Incoming messages processor
        private readonly IIncomingMessagesProcessor _processor;

        // Home Assistant discovery publisher
        private readonly HomeAssistantDiscoveryPublisher _haDiscoveryEnumerator;

        private readonly Win2MqttOptions _options;
        private readonly ILogger<WindowsBackgroundService> _logger;

        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        public WindowsBackgroundService(IMqttConnector connector,
                                              ISensorDataCollector collector,
                                              IIncomingMessagesProcessor messagesProcessor,
                                              HomeAssistantDiscoveryPublisher haDiscoveryEnumerator,
                                              IOptions<Win2MqttOptions> options,
                                              ILogger<WindowsBackgroundService> logger)
        {
            _connector = connector;
            _collector = collector;
            _processor = messagesProcessor;
            _haDiscoveryEnumerator = haDiscoveryEnumerator;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started");
            try
            {
                // Connect to MQTT broker
                if (await _connector.ConnectAsync())
                {
                    // Publish Home Assistant discovery messages
                    await _haDiscoveryEnumerator.PublishSensorsDiscoveryAsync();

                    // Subscribe to incoming messages
                    // Process them with IIncomingMessagesProcessor.ProcessMessageAsync()
                    if (await _connector.SubscribeAsync(_processor.ProcessMessageAsync))
                    {
                        await _connector.PublishToFullTopicAsync("status", "online", retain: true);

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
                                    try
                                    {
                                        await _connector.PublishMessageAsync(sensorData.Key, sensorData.Value);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, "Failed to publish sensor {Sensor}", sensorData.Key);
                                    }
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
                else
                {
                    _logger.LogWarning("MQTT connection failed. Retrying in 10 seconds...");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    return;
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

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connector.PublishMessageAsync("status", "offline", retain: true);
            await _connector.DisconnectAsync();
            await base.StopAsync(cancellationToken);
        }

    }
}
