using System.Reflection;
using Faceira.Shared.Application.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Service.Installers;

public static class HandlersInstaller
{
    public static IServiceCollection AddHandlersAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            services.AddHandlersAssembly(assembly);
        }

        return services;
    }
    
    private static IServiceCollection AddHandlersAssembly(this IServiceCollection services, Assembly assembly)
    {
        services.AddGenericImplementations(assembly, typeof(IHandle<>));
        services.AddGenericImplementations(assembly, typeof(IMapper<>));

        return services;
    }

    private static IServiceCollection AddGenericImplementations(this IServiceCollection services, 
        Assembly assembly, Type type)
    {
        var implementations = assembly.GetGenericImplementations(type);

        foreach (var implementation in implementations)
        {
            services.AddScoped(
                implementation.GetImplementedInterface(), 
                implementation);
        }

        return services;
    }
}