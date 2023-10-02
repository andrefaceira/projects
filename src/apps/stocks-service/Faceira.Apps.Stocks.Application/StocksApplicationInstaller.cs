using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Persistence;
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
        services.AddServiceBus(configuration.ServiceBuses.StocksServiceBus.Name);
        services.AddDbContext<StocksContext>(options =>
            options.UseNpgsql(configuration.Databases.StocksDatabase.ConnectionString));
        services.AddHttpClient<IFinnhubHttpClient, FinnhubHttpClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.Apis.FinnhubApi.Url);
            
            var httpHeaders = configuration.Apis.FinnhubApi.HttpHeaders ?? new Dictionary<string, string>();
            foreach (var httpHeader in httpHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(
                    httpHeader.Key, httpHeader.Value);
            }
        });
        
        return services;
    }
}