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
        // dispatcher
        services.AddScoped<IDispatcher>(serviceProvider =>
            new DefaultDispatcher(serviceProvider));
            
        // dapr
        services.AddScoped<DaprClient, DaprClient>(_ =>
                new DaprClientBuilder().Build());

        // logging
        services.AddLogging(logs =>
            logs.AddConsole());
        
        // tracing
        services.AddOpenTracing(p =>
        {
            p.ConfigureAspNetCore(options =>
            {
                options.Hosting.IgnorePatterns.Add(ctx => ctx.Request.Path == "/health");
            });
        });
        
        return services;
    }
}