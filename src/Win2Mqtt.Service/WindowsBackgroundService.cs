namespace Win2Mqtt.Service
{
    public class WindowsBackgroundService(
        Win2MqttService service, 
        ILogger<WindowsBackgroundService> logger) : BackgroundService
    {

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Worker started");

            // Connect to MQTT broker
            await service.ConnectToMqttBrokerAsync(cancellationToken);

            // Subscribe to incoming messages and  process them with IIncomingMessagesProcessor.ProcessMessageAsync()
            await service.SubscribeToIncomingMessagesAsync(cancellationToken);

            // Publish Home Assistant discovery messages and online status
            await service.StartAsync(cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await service.CollectAndPublish(stoppingToken);
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
        }
    }
}
