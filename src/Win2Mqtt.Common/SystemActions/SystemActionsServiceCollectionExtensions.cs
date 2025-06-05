using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Win2Mqtt.SystemActions
{
    public static class SystemActionsServiceCollectionExtensions
    {
        public static IServiceCollection AddSystemActionsFromAssembly(this IServiceCollection services, Assembly assembly) =>
            services.Scan(scan => scan
            .FromAssemblyDependencies(assembly)
            .AddClasses(classes => classes.AssignableTo<ISystemActionWrapper>())
            .AsSelf()
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        }
}
