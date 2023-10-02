using System.Globalization;
using System.Text.Json;
using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Messages.Financials;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers;

public class UpdateFinancialsYear : IHandle<FinancialsYearUpdateTriggered>
{
    private readonly StocksContext _stocksContext;
    private readonly IFinnhubHttpClient _httpClient;

    public UpdateFinancialsYear(StocksContext stocksContext, IFinnhubHttpClient httpClient)
    {
        _stocksContext = stocksContext;
        _httpClient = httpClient;
    }

    public async Task Handle(FinancialsYearUpdateTriggered message)
    {
        var reports = await Get(message.Symbol);
        await Update(message.Symbol, reports);
    }

    private async Task<IEnumerable<ReportUpdated>> Get(string symbol)
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
                BalanceSheet = p.GetProperty("report").GetProperty("bs").EnumerateArray(),
                IncomeStatement = p.GetProperty("report").GetProperty("ic").EnumerateArray(),
                CashFlow = p.GetProperty("report").GetProperty("cf").EnumerateArray(),
            })
            .Select(p => new ReportUpdated(
                p.Symbol,
                p.Year,
                p.Quarter,
                p.PeriodStart,
                p.PeriodEnd,
                
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                
                0,
                0,
                0,
                0
            ));
        
        return reports;
    }

    private async Task Update(string symbol, IEnumerable<ReportUpdated> reports)
    {
        var lastFinancial = _stocksContext.FinancialsYears
            .Where(p => p.Symbol == symbol)
            .OrderBy(p => p.Year)
            .ThenBy(p => p.Quarter)
            .FirstOrDefault();

        var newFinancials = reports
            .Where(p => p.Year > lastFinancial?.Year || p.Quarter > lastFinancial?.Quarter);

        if (!newFinancials.Any())
        {
            return;
        }
        
        await _stocksContext.AddRangeAsync(newFinancials);
        await _stocksContext.SaveChangesAsync();
    }
}
