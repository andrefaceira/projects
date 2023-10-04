using System.Text.Json;
using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Messages.Financials;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers.Financials;

public class UpdateFinancialsQuarter : IHandle<FinancialsQuarterUpdateTriggered>
{
    private readonly IFinnhubHttpClient _httpClient;
    private readonly IMapper<IEnumerable<FinancialReport>> _mapper;
    private readonly StocksContext _stocksContext;
    private readonly IServiceBus _serviceBus;

    public UpdateFinancialsQuarter(IFinnhubHttpClient httpClient, IMapper<IEnumerable<FinancialReport>> mapper, 
        StocksContext stocksContext, IServiceBus serviceBus)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _stocksContext = stocksContext;
        _serviceBus = serviceBus;
    }
    
    public async Task Handle(FinancialsQuarterUpdateTriggered message)
    {
        var response = await _httpClient.Get<JsonElement>(
            $"stock/financials-reported?symbol={message.Symbol}&freq=quarterly");

        if (response.GetProperty("symbol").GetString() != message.Symbol)
        {
            throw new InvalidOperationException(
                $"HttpClient returned a company with symbol {response.GetProperty("symbol").GetString()} instead of {message.Symbol}");
        }
        
        var reports = _mapper.Map(response);
        
        var lastFinancial = _stocksContext.Financials
            .Where(p => p.Symbol == message.Symbol)
            .Where(p => p.Type == FinancialReport.ReportTypeNominal)
            .Where(p => p.Quarter > 0)
            .OrderBy(p => p.Year)
            .ThenBy(p => p.Quarter)
            .FirstOrDefault();

        var newFinancials = reports
            .Where(p => p.Year >= lastFinancial?.Year || 
                        (p.Year == lastFinancial?.Year && p.Quarter > lastFinancial.Quarter));

        if (!newFinancials.Any())
        {
            return;
        }
        
        await _stocksContext.AddRangeAsync(newFinancials);
        await _stocksContext.SaveChangesAsync();

        await _serviceBus.Publish(
            new FinancialsUpdated(reports));
    }
}