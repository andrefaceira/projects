using System.Text.Json;
using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Messages;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers;

public class FetchFinancialsFinnhub : IHandle<FinancialsUpdateTriggered>
{
    private readonly StocksContext _stocksContext;
    private readonly IFinnhubHttpClient _httpClient;

    public FetchFinancialsFinnhub(StocksContext stocksContext, IFinnhubHttpClient httpClient)
    {
        _stocksContext = stocksContext;
        _httpClient = httpClient;
    }

    public async Task Handle(FinancialsUpdateTriggered message)
    {
        var symbol = message.Symbol;
        
        var response = await _httpClient.Get<JsonElement>(
            $"stock/financials-reported?symbol={symbol}");

        if (response.GetProperty("symbol").GetString() != symbol)
        {
            throw new InvalidOperationException(
                $"FinnhubHttpClient returned a company with symbol {response.GetProperty("ticker").GetString()} instead of {symbol}");
        }

        var reports = response.GetProperty("data")
            .EnumerateArray()
            .Select(p => new
            {
                Symbol = p.GetProperty("symbol").ToString(),
                Year = p.GetProperty("year").GetInt32(),
                Quarter = p.GetProperty("quarter").GetInt32(),
                PeriodStart = DateTime.SpecifyKind(p.GetProperty("startDate").GetDateTime(), DateTimeKind.Utc),
                PeriodEnd = DateTime.SpecifyKind(p.GetProperty("endDate").GetDateTime(), DateTimeKind.Utc),
                BalanceSheet = p.GetProperty("report").GetProperty("bs").EnumerateArray(),
                IncomeStatement = p.GetProperty("report").GetProperty("ic").EnumerateArray(),
                CashFlow = p.GetProperty("report").GetProperty("cf").EnumerateArray(),
            });

        var x = reports.ToList();
    }
}