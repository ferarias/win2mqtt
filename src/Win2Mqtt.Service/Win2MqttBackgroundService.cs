using Microsoft.Extensions.Options;
using Win2Mqtt.Common;
using Win2Mqtt.Common.Options;
using Win2Mqtt.Infra.HomeAssistant;

namespace Win2Mqtt.Service
{
    public class Win2MqttBackgroundService : BackgroundService
    {
        // MQTT connector
        private readonly IMqttConnectionManager _mqttConnectionManager;
        private readonly IMqttPublisher _mqttPublisher;
        private readonly IMqttSubscriber _mqttSubscriber;

        // System information collector
        private readonly ISensorDataCollector _sensorDataCollector;

        // Incoming messages processor
        private readonly IIncomingMessagesProcessor _incomingMessagesProcessor;

        // Home Assistant discovery publisher
        private readonly IHomeAssistantDiscoveryPublisher _haDiscoveryPublisher;

        private readonly Win2MqttOptions _options;
        private readonly ILogger<Win2MqttBackgroundService> _logger;

        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        public Win2MqttBackgroundService(
            IMqttConnectionManager mqttConnectionManager,
            IMqttSubscriber mqttSubscriber,
            IMqttPublisher mqttPublisher,
            ISensorDataCollector sensorDataCollector,
            IIncomingMessagesProcessor incomingMessagesProcessor,
            IHomeAssistantDiscoveryPublisher haDiscoveryPublisher,
            IOptions<Win2MqttOptions> options,
            ILogger<Win2MqttBackgroundService> logger)
        {
            _mqttConnectionManager = mqttConnectionManager;
            _mqttSubscriber = mqttSubscriber;
            _mqttPublisher = mqttPublisher;
            _sensorDataCollector = sensorDataCollector;
            _incomingMessagesProcessor = incomingMessagesProcessor;
            _haDiscoveryPublisher = haDiscoveryPublisher;
            _options = options.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started");
            try
            {
                // Connect to MQTT broker
                await ConnectToMqttBrokerAsync(stoppingToken);

                // Subscribe to incoming messages and  process them with IIncomingMessagesProcessor.ProcessMessageAsync()
                await SubscribeToIncomingMessagesAsync(stoppingToken);

                // Publish Home Assistant discovery messages
                await _haDiscoveryPublisher.PublishSensorsDiscoveryAsync(stoppingToken);

                var statusTopic = $"{Constants.ServiceBaseTopic}/{_options.MachineIdentifier}/status";
                await _mqttPublisher.PublishAsync(statusTopic, "online", retain: true, stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    // Allow only one thread collecting system information
                    await _semaphore.WaitAsync(stoppingToken);
                    try
                    {
                        // Collect system information
                        var sensorsData = await _sensorDataCollector.CollectSystemDataAsync();

                        // Publish collected data
                        foreach (var sensorData in sensorsData)
                        {
                            try
                            {
                                var sensorTopic = $"{Constants.ServiceBaseTopic}/{_options.MachineIdentifier}/{sensorData.Key}";
                                await _mqttPublisher.PublishAsync(sensorTopic, sensorData.Value, false, cancellationToken: stoppingToken);
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
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
                _logger.LogInformation("Cancelling pending operations.");
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

        private async Task ConnectToMqttBrokerAsync(CancellationToken stoppingToken)
        {
            while (!(await _mqttConnectionManager.ConnectAsync(stoppingToken)))
            {
                _logger.LogWarning("MQTT connection failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        private async Task SubscribeToIncomingMessagesAsync(CancellationToken stoppingToken)
        {
            while (!(await _mqttSubscriber.SubscribeAsync(_incomingMessagesProcessor.ProcessMessageAsync, stoppingToken)))
            {
                _logger.LogWarning("MQTT subscription failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            var statusTopic = $"{Constants.ServiceBaseTopic}/{_options.MachineIdentifier}/status";
            await _mqttPublisher.PublishAsync(statusTopic, "offline", retain: true, cancellationToken: cancellationToken);
            await _mqttConnectionManager.DisconnectAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
