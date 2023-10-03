using Dapr.Client;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Application.ServiceBuses;
using Faceira.Shared.Application.Dapr;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Service.Installers;

public static class ServiceBusInstaller
{
    public static IServiceCollection AddServiceBus(this IServiceCollection services,
        string bindingName)
    {
        services.AddScoped<IServiceBus, DaprServiceBus>(serviceProvider =>
        {
            var daprClient = serviceProvider.GetRequiredService<DaprClient>();
            return new DaprServiceBus(daprClient, bindingName);
        });
        
        return services;
    } 
}