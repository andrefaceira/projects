using System.Reflection;
using Faceira.Shared.Application.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Service.Installers;

public static class HandlersInstaller
{
    public static IServiceCollection AddHandlers(this IServiceCollection services,
        Assembly assembly)
    {
        var handlers = assembly
            .GetTypes()
            .Where(p => p.GetInterfaces()
                .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == typeof(IHandle<>)))
            .Select(p => new
            {
                HandlerType = p,
                InterfaceType = p.GetInterfaces().First()
            });

        foreach (var handler in handlers)
        {
            services.AddScoped(handler.InterfaceType, handler.HandlerType);
        }

        return services;
    }
}