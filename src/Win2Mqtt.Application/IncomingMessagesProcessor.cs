using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public class IncomingMessagesProcessor(
       IServiceProvider sp,
       ISensorValueFormatter formatter,
       IMqttPublisher publisher,
       IActionFactory actionFactory,
       IOptions<Win2MqttOptions> options,
       ILogger<IncomingMessagesProcessor> logger) : IIncomingMessagesProcessor
    {
        private readonly Win2MqttOptions _options = options.Value;
        private static readonly ConcurrentDictionary<Type, MethodInfo> _methodCache = new();
        private static readonly ConcurrentDictionary<Type, PropertyInfo?> _resultPropertyCache = new();

        public async Task ProcessMessageAsync(string subtopic, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var actions = actionFactory.GetEnabledActions();
                if (!actions.TryGetValue(subtopic, out IMqttActionHandlerMarker? handler))
                {
                    logger.LogWarning("No enabled listener for subtopic `{Subtopic}`", subtopic);
                    return;
                }

                var handlerType = handler.GetType();
                // Determine if it's IMqttActionHandler or IMqttActionHandler<T>
                if (handlerType.GetInterfaces().FirstOrDefault(i => i == typeof(IMqttActionHandler)
                    || (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMqttActionHandler<>))) == null)
                {
                    logger.LogWarning("Handler `{Handler}` does not implement a valid IMqttActionHandler interface", handlerType.Name);
                    return;
                }
                // Cached retrieval of HandleAsync method
                var method = _methodCache.GetOrAdd(handlerType, static type => 
                    type.GetMethod("HandleAsync", [typeof(string), typeof(CancellationToken)])
                    ?? throw new InvalidOperationException($"HandleAsync not found in {type}"));

                var task = (Task)method.Invoke(handler, [message, cancellationToken])!;
                await task.ConfigureAwait(false);

                object? result = null;
                if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var resultProperty = _resultPropertyCache.GetOrAdd(method.ReturnType, static type =>
                        type.GetProperty("Result"));

                    result = resultProperty?.GetValue(task);
                }

                if (result != null)
                {
                    logger.LogTrace("Handler `{Handler}` returned: {Result}", handlerType.Name, result);
                    await publisher.PublishAsync(
                        topic: $"{_options.MqttBaseTopic}/{handler.Metadata.CommandTopic}/result",
                        message: formatter.Format(result), 
                        retain: false, 
                        cancellationToken: cancellationToken);
                }
                else
                {
                    logger.LogTrace("Handler `{Handler}` completed without a result", handlerType.Name);
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on processing message received");
            }
        }
    }
}
