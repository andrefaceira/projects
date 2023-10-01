using Dapr.Client;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Dapr;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Service.Installers;

public static class HttpClientInstaller
{
    public static IServiceCollection AddHttpClient(this IServiceCollection services,
        string key, string bindingName)
    {
        services.AddKeyedScoped<IHttpClient, DaprHttpClient>(key, (serviceProvider, _) =>
        {
            var daprClient = serviceProvider.GetRequiredService<DaprClient>();
            return new DaprHttpClient(daprClient, bindingName);
        });
        
        return services;
    } 
}