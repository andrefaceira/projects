using System.Reflection;
using Faceira.Shared.Application.Service.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application;

public static class DefaultApplicationBuilder
{
    public static WebApplicationBuilder CreateApiBuilder(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);
        
        builder.Services.AddApi();
        builder.Services.AddApplication();

        return builder;
    }
    
    public static IServiceCollection AddHandlersAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            services.AddHandlersAssemblies(assembly);
        }

        return services;
    }
    
    public static WebApplication BuildApi(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        // health checks
        app.MapHealthChecks("/health");
        
        // dapr
        app.UseCloudEvents();
        app.MapSubscribeHandler();

        // swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}