using System.Globalization;
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
        var reports = await GetFinancials(message.Symbol);

        var x = reports.ToList();
    }

    private async Task<IEnumerable<FinancialsUpdated>> GetFinancials(string symbol)
    {
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
                PeriodStart = DateTime.ParseExact(
                    p.GetProperty("startDate").ToString(),
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None),
                PeriodEnd = DateTime.ParseExact(
                    p.GetProperty("endDate").ToString(),
                    "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None),
                // PeriodEnd = DateTime.SpecifyKind(p.GetProperty("endDate").GetDateTime(), DateTimeKind.Utc),
                BalanceSheet = p.GetProperty("report").GetProperty("bs").EnumerateArray(),
                IncomeStatement = p.GetProperty("report").GetProperty("ic").EnumerateArray(),
                CashFlow = p.GetProperty("report").GetProperty("cf").EnumerateArray(),
            })
            .Select(p => new FinancialsUpdated(
                p.Symbol,
                p.Year,
                p.Quarter,
                p.PeriodStart,
                p.PeriodEnd
            ));
        
        return reports;
    }
}