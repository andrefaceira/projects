using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Application.ServiceBuses;
using Faceira.Shared.Application.Service.Installers;
using Faceira.Shared.Application.Service.Installers.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Apps.Stocks.Application;

public static class StocksApplicationInstaller
{
    public static IServiceCollection AddStocksApplication(this IServiceCollection services, 
        StocksApplicationConfiguration configuration)
    {
        services.AddApplication();
        services.AddScoped<IServiceBus, NoopServiceBus>();
        // services.AddServiceBus(configuration.ServiceBuses.StocksServiceBus.Name);
        services.AddDbContext<StocksContext>(options =>
            options.UseNpgsql(configuration.Databases.StocksDatabase.ConnectionString));
        services.AddDaprHttpClient("finnhub", configuration.Apis.FinnhubApiBinding);
        
        return services;
    }
}