using System.Collections.Concurrent;
using System.Reflection;
using Win2Mqtt.SystemActions;
using Win2Mqtt.SystemSensors;

namespace Win2Mqtt.Application
{
    public sealed class GenericMqttActionHandlerWrapper(
        IMqttActionHandlerMarker handler,
        IMqttPublisher publisher,
        ISensorValueFormatter formatter,
        string topic)
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> _methodCache = new();
        private static readonly ConcurrentDictionary<Type, PropertyInfo?> _resultPropertyCache = new();

        public string HandlerKey { get; } = handler.GetType().Name.Replace("Handler", "");

        public async Task HandleAsync(string payload, CancellationToken cancellationToken)
        {
            var handlerType = handler.GetType();

            var method = _methodCache.GetOrAdd(handlerType,
                static type => type.GetMethod("HandleAsync") ?? throw new InvalidOperationException($"HandleAsync not found in {type}"));

            var task = (Task)method.Invoke(handler, [payload, cancellationToken])!;
            await task.ConfigureAwait(false);

            if (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultProperty = _resultPropertyCache.GetOrAdd(task.GetType(),
                    static type => type.GetProperty("Result"));

                var result = resultProperty?.GetValue(task);
                if (result != null)
                {
                    var resultPayload = formatter.Format(result);
                    await publisher.PublishAsync(topic, resultPayload, retain: false, cancellationToken);
                }
            }
        }
    }
}
