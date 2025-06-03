using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public class Win2MqttBackgroundService(
        IMqttConnectionManager connectionManager,
        IActionFactory actionFactory,
        ISensorFactory sensorFactory,
        IMessagePublisher publisher,
        IMqttSubscriber subscriber,
        IOptionsMonitor<Win2MqttOptions> options,
        ILogger<Win2MqttBackgroundService> logger) : BackgroundService
    {
        private readonly static SemaphoreSlim _semaphore = new(1, 1);
        private readonly IEnumerable<ISensorWrapper> _activeSensors = sensorFactory.GetEnabledSensors();
        private readonly IEnumerable<IMqttActionHandlerMarker> _activeActions = actionFactory.GetEnabledActions().Select(kv => kv.Value);


        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            // Connect to MQTT broker, Subscribe to incoming messages and Publish Home Assistant discovery messages/online status
            // Connect to MQTT broker
            await connectionManager.ConnectAsync(stoppingToken);

            // Subscribe to incoming messages
            await subscriber.SubscribeAsync(stoppingToken);

            // Publish Home Assistant sensor discovery messages
            foreach (var sensor in _activeSensors)
            {
                await publisher.PublishSensorDiscoveryMessage(sensor.Metadata, stoppingToken);
            }
            // Publish Home Assistant switch discovery messages
            foreach (var action in _activeActions)
            {
                await publisher.PublishSwitchDiscoveryMessage(action.Metadata, stoppingToken);
            }

            // Publish online status
            await publisher.PublishOnlineStatus(stoppingToken);

            await base.StartAsync(stoppingToken);
            logger.LogInformation("Worker started");

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // Allow only one thread collecting system information
                    await _semaphore.WaitAsync(stoppingToken);
                    try
                    {

                        foreach (var sensor in _activeSensors)
                        {
                            var sensorValue = await sensor.CollectAsync();
                            await publisher.PublishSensorValue(sensor, sensorValue, stoppingToken);
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                    await Task.Delay(TimeSpan.FromSeconds(options.CurrentValue.TimerInterval), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
                logger.LogInformation("Cancelling pending operations.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);

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
            // Disonnect from MQTT broker
            await publisher.PublishOfflineStatus(cancellationToken);

            // Publish offline status
            await connectionManager.DisconnectAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            logger.LogInformation("Worker stopped.");
        }
    }
}
