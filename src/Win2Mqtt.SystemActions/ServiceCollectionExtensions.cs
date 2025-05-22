using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemActions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemActions(this IServiceCollection services) =>
            services.Scan(scan => scan
            .FromAssemblies([typeof(ServiceCollectionExtensions).Assembly])
            .AddClasses(classes => classes.AssignableTo<IMqttActionHandlerMarker>())
			.AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }
}
