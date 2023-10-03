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
        var assembly = Assembly.GetCallingAssembly();
        
        return services
            .AddGenericImplementations(assembly, typeof(IHandle<>))
            .AddGenericImplementations(assembly, typeof(IMapper<>))
            .AddScoped<IDispatcher>(serviceProvider =>
                new ExceptionsDispatcher(
                    new DefaultDispatcher(serviceProvider)))
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

    private static IServiceCollection AddGenericImplementations(this IServiceCollection services, 
        Assembly assembly, Type type)
    {
        var implementations = assembly
            .GetTypes()
            .Where(p => p.GetInterfaces()
                .Any(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == type))
            .Select(p => new
            {
                Type = p,
                InterfaceType = p.GetInterfaces().First()
            });

        foreach (var implementation in implementations)
        {
            services.AddScoped(implementation.InterfaceType, implementation.Type);
        }

        return services;
    }
}