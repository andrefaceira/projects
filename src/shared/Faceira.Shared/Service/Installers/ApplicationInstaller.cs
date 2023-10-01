using System.Reflection;
using Dapr.Client;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Application.Dispatchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Faceira.Shared.Application.Service.Installers;

public static class ApplicationInstaller
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddHandlers(Assembly.GetCallingAssembly())
            .AddScoped<IDispatcher>(serviceProvider =>
                new DefaultDispatcher(serviceProvider))
            .AddScoped<DaprClient, DaprClient>(_ =>
                new DaprClientBuilder().Build())
            .AddLogging(logs => { logs.AddConsole(); })
            .AddOpenTracing(p =>
            {
                p.ConfigureAspNetCore(options =>
                {
                    options.Hosting.IgnorePatterns.Add(ctx => ctx.Request.Path == "/health");
                });
            });
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services,
        Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

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