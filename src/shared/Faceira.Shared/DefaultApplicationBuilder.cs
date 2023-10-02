using System.Reflection;
using Faceira.Shared.Application.Service.Installers;
using Microsoft.AspNetCore.Builder;

namespace Faceira.Shared.Application;

public static class DefaultApplicationBuilder
{
    public static WebApplicationBuilder CreateApiBuilder(string[] args,
        IEnumerable<Assembly>? assemblies = null)
    {
        var builder = WebApplication.CreateSlimBuilder(args);
        
        builder.Services.AddApi();
        builder.Services.AddApplication();
        builder.Services.AddHandlers(assemblies);

        return builder;
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