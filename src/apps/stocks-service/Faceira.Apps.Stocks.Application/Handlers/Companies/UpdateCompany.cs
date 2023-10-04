using System.Text.Json;
using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Messages.Companies;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Apps.Stocks.Application.Handlers.Companies;

public class UpdateCompany : IHandle<CompanyUpdateTriggered>
{
    private readonly StocksContext _stocksContext;
    private readonly IHttpClient _httpClient;
    private readonly IServiceBus _serviceBus;

    public UpdateCompany(StocksContext stocksContext, 
        [FromKeyedServices(DaprHttpClients.Finnhub)] IHttpClient httpClient,
        IServiceBus serviceBus)
    {
        _stocksContext = stocksContext;
        _httpClient = httpClient;
        _serviceBus = serviceBus;
    }

    public async Task Handle(CompanyUpdateTriggered message)
    {
        var companyUpdated = await Get(message.Symbol);
        
        await Update(companyUpdated);
        
        await _serviceBus.Publish(companyUpdated);
    }

    private async Task<CompanyUpdated> Get(string symbol)
    {
        var response = await _httpClient.Get<JsonElement>(
            $"stock/profile2?symbol={symbol}");

        if (response.GetProperty("ticker").GetString() != symbol)
        {
            throw new InvalidOperationException(
                $"FinnhubHttpClient returned a company with symbol {response.GetProperty("ticker").GetString()} instead of {symbol}");
        }
        
        return new CompanyUpdated(
            response.GetProperty("ticker").ToString(),
            response.GetProperty("name").ToString(),
            response.GetProperty("exchange").ToString(),
            response.GetProperty("currency").ToString(),
            response.GetProperty("country").ToString(),
            DateTime.SpecifyKind(response.GetProperty("ipo").GetDateTime(), DateTimeKind.Utc),
            response.GetProperty("finnhubIndustry").ToString()
        );
    }

    private async Task Update(CompanyUpdated companyUpdated)
    {
        var companyExists = _stocksContext.Companies.Any(p => p.Symbol == companyUpdated.Symbol);
        if (companyExists)
        {
            _stocksContext.Update(companyUpdated);
        }
        else
        {
            await _stocksContext.AddAsync(companyUpdated);
        }

        await _stocksContext.SaveChangesAsync();
    }
}