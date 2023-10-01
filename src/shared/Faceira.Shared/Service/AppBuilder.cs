using Microsoft.AspNetCore.Builder;

namespace Faceira.Shared.Application.Service;

public static class AppBuilder
{
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