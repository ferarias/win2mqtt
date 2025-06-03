using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemActions.Windows
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWindowsSpecificSystemActions(this IServiceCollection services) =>
            services.Scan(scan => scan
            .FromAssemblies([typeof(ServiceCollectionExtensions).Assembly])
            .AddClasses(classes => classes.AssignableTo<ISystemActionWrapper>())
			.AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }
}
