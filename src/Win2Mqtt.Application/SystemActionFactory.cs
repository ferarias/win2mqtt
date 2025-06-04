using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Win2Mqtt.Common;
using Win2Mqtt.Options;
using Win2Mqtt.SystemActions;

namespace Win2Mqtt.Application
{
    public class SystemActionFactory(
    IOptions<Win2MqttOptions> options,
    IServiceProvider serviceProvider,
    ILogger<SystemActionFactory> logger) : ISystemActionFactory
    {
        private readonly Win2MqttOptions _options = options.Value;
        private static readonly Dictionary<string, (Type HandlerType, PropertyInfo? ReturnProperty)> _handlers = [];

        public IDictionary<string, ISystemActionWrapper> GetEnabledActions()
        {
            var actions = new Dictionary<string, ISystemActionWrapper>(StringComparer.OrdinalIgnoreCase);
            foreach (var (listenerName, listenerOptions) in _options.Listeners)
            {
                if (listenerOptions.Enabled)
                {
                    var handlerType = GetHandlerTypeByKey(listenerName);
                    if (handlerType == null)
                    {
                        logger.LogWarning("No handler registered for listener `{Listener}`", listenerName);
                        continue;
                    }
                    if (serviceProvider.GetRequiredService(handlerType) is ISystemActionWrapper handler)
                    {
                        // if topic is set in options, overwrite what's set in attribute
                        var topicName = string.IsNullOrWhiteSpace(listenerOptions.Topic) 
                            ? SanitizeHelpers.Sanitize(listenerName) 
                            : SanitizeHelpers.Sanitize(listenerOptions.Topic);

                        var uniqueId = $"{_options.DeviceUniqueId}_{SanitizeHelpers.Sanitize(listenerName)}";
                        handler.Metadata = new SystemActionMetadata
                        {
                            Key = listenerName,
                            Name = handlerType.Name.Replace("Handler", string.Empty),
                            UniqueId = uniqueId,
                            StateTopic = $"{_options.MqttBaseTopic}/{topicName}",
                            CommandTopic = $"{_options.MqttBaseTopic}/{topicName}"
                        };
                        actions.Add(listenerName, handler);
                    }
                }
            }
            return actions;
        }

        private static Type? GetHandlerTypeByKey(string key)
        {
            var typeName = key + "Handler";
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase)
                                  && (typeof(ISystemAction).IsAssignableFrom(t) || ImplementsIMqttActionHandlerGeneric(t)));

            return type ?? null;
        }

        private static bool ImplementsIMqttActionHandlerGeneric(Type type)
        {
            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystemAction<>));
        }

    }
}
