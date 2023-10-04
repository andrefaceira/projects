using Dapr.Client;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Application.ServiceBuses;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Service.Installers.Modules;

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