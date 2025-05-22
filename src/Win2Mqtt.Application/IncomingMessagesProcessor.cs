using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
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
                var match = _options
                    .Listeners
                    .FirstOrDefault(kv => kv.Value.Topic.Equals(subtopic, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(match.Key) || !match.Value.Enabled)
                {
                    logger.LogWarning("No enabled listener for subtopic `{Subtopic}`", subtopic);
                    return;
                }

                var handlerType = GetHandlerTypeByKey(match.Key);
                if (handlerType == null)
                {
                    logger.LogWarning("No handler registered for listener `{Listener}`", match.Key);
                    return;
                }
                var handler = sp.GetRequiredService(handlerType);
                // Determine if it's IMqttActionHandler or IMqttActionHandler<T>
                var validInterface = handlerType
                    .GetInterfaces()
                    .FirstOrDefault(i => i == typeof(IMqttActionHandler) 
                                || (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMqttActionHandler<>)));
                if (validInterface == null)
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
                    string topic = handler.GetType().Name.Replace("Handler", "").ToLower();
                    var resultTopic = $"{_options.MqttBaseTopic}/{topic}/result";
                    var resultPayload = formatter.Format(result);
                    await publisher.PublishAsync(resultTopic, resultPayload, false, cancellationToken);
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on processing message received");
            }
        }

        private static Type GetHandlerTypeByKey(string key)
        {
            var typeName = key + "Handler";
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                                  && (typeof(IMqttActionHandler).IsAssignableFrom(t) || ImplementsIMqttActionHandlerGeneric(t)));

            return type ?? throw new InvalidOperationException($"No handler found for {key}");
        }

        private static bool ImplementsIMqttActionHandlerGeneric(Type type)
        {
            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMqttActionHandler<>));
        }


    }
}
