using System.Reflection;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Messages;
using Faceira.Shared.Application.Service.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
            services.AddHandlersAssembly(assembly);
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
    
    public static WebApplication BuildRoutes(this WebApplication app, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var controllers = assembly
                .GetGenericImplementations(typeof(IHandle<>))
                .GroupBy(p => p.Namespace);
            
            foreach (var controller in controllers)
            {
                var group = app.MapGroup(
                    controller.Key?.Split('.').Last() ?? string.Empty);

                foreach (var endpoint in controller)
                {
                    var type = endpoint.GetGenericImplementationType();
                    
                    typeof(DefaultApplicationBuilder)
                        .InvokeGenericMethod(
                            nameof(BuildServiceApiRoute),
                            type,
                            new object[] { group, type.Name });
                }
            }
        }

        return app;
    }
    
    public static RouteGroupBuilder BuildServiceApiRoute<TMessage>(this RouteGroupBuilder routeBuilder, string path)
        where TMessage : IMessage
    {
        routeBuilder.MapPost(
            path,
            async (TMessage message, IDispatcher dispatcher) => 
            {
                await dispatcher.Dispatch(message);
    
                return Results.Ok();
            });
        
        return routeBuilder;
    }
}