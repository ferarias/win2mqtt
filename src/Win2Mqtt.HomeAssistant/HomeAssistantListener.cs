using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Win2Mqtt.SystemActions;

namespace Win2Mqtt.HomeAssistant
{

    public class HomeAssistantListener(
        IMqttSubscriber mqttSubscriber,
        IIncomingMessagesProcessor incomingMessagesProcessor,
        ILogger<HomeAssistantListener> logger) 
        : IMessageListener
    {

        public async Task SubscribeToIncomingMessagesAsync(CancellationToken stoppingToken)
        {
            while (!(await mqttSubscriber.SubscribeAsync(incomingMessagesProcessor.ProcessMessageAsync, stoppingToken)))
            {
                logger.LogWarning("MQTT subscription failed. Retrying in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
