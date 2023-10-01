using System.Text.Json;
using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Messages;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers;

public class FetchCompanyDetailsFinnhub : IHandle<CompanyUpdateTriggered>
{
    private readonly StocksContext _stocksContext;
    private readonly IFinnhubHttpClient _httpClient;

    public FetchCompanyDetailsFinnhub(StocksContext stocksContext, 
        IFinnhubHttpClient httpClient)
    {
        _stocksContext = stocksContext;
        _httpClient = httpClient;
    }

    public async Task Handle(CompanyUpdateTriggered message)
    {
        var companyUpdated = await GetCompany(message.Symbol);

        await UpdateCompany(companyUpdated);
    }

    private async Task<CompanyUpdated> GetCompany(string symbol)
    {
        var response = await _httpClient.Get<JsonElement>(
            $"stock/profile2?symbol={symbol}");

        if (response.GetProperty("ticker").GetString() != symbol)
        {
            throw new InvalidOperationException(
                $"FinnhubHttpClient.Get<CompanyUpdated> returned a company with symbol {response.GetProperty("ticker").GetString()} instead of {symbol}");
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

    private async Task UpdateCompany(CompanyUpdated companyUpdated)
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