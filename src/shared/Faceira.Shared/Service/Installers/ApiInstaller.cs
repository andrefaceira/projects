using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Service.Installers;

public static class ApiInstaller
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // health checks
        services.AddHealthChecks();

        // swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}