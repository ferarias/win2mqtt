using Microsoft.Extensions.Options;
using Win2Mqtt.Application;
using Win2Mqtt.Options;

namespace Win2Mqtt.Service
{
    public class WindowsBackgroundService(
        Win2MqttService service,
        IOptionsMonitor<Win2MqttOptions> options,
        ILogger<WindowsBackgroundService> logger) : BackgroundService
    {
        private readonly static SemaphoreSlim _semaphore = new(1, 1);


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Worker started");

            // Connect to MQTT broker, Subscribe to incoming messages and Publish Home Assistant discovery messages/online status
            await service.StartAsync(cancellationToken);

            await base.StartAsync(cancellationToken);
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

                        await service.CollectAndPublish(stoppingToken);
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
            await service.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
            logger.LogInformation("Worker stopped.");
        }
    }
}
