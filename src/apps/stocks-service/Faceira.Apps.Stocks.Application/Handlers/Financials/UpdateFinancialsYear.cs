using System.Globalization;
using System.Text.Json;
using Faceira.Apps.Stocks.Application.Handlers.Financials.Mappers;
using Faceira.Apps.Stocks.Application.HttpClients;
using Faceira.Apps.Stocks.Messages.Financials;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers.Financials;

public class UpdateFinancialsYear : IHandle<FinancialsYearUpdateTriggered>
{
    private readonly IFinnhubHttpClient _httpClient;
    private readonly IMapper<IEnumerable<ReportUpdated>> _mapper;
    private readonly StocksContext _stocksContext;
    private readonly IServiceBus _serviceBus;

    public UpdateFinancialsYear(IFinnhubHttpClient httpClient, IMapper<IEnumerable<ReportUpdated>> mapper, 
        StocksContext stocksContext, IServiceBus serviceBus)
    {
        _httpClient = httpClient;
        _mapper = mapper;
        _stocksContext = stocksContext;
        _serviceBus = serviceBus;
    }

    public async Task Handle(FinancialsYearUpdateTriggered message)
    {
        var response = await _httpClient.Get<JsonElement>(
            $"stock/financials-reported?symbol={message.Symbol}&freq=annual");

        if (response.GetProperty("symbol").GetString() != message.Symbol)
        {
            throw new InvalidOperationException(
                $"HttpClient returned a company with symbol {response.GetProperty("symbol").GetString()} instead of {message.Symbol}");
        }
        
        var reports = _mapper.Map(response);
        
        var lastFinancial = _stocksContext.FinancialsYears
            .Where(p => p.Symbol == message.Symbol)
            .OrderBy(p => p.Year)
            .ThenBy(p => p.Quarter)
            .FirstOrDefault();
        
        var newFinancials = reports
            .Where(p => p.Year > lastFinancial?.Year);
        
        if (!newFinancials.Any())
        {
            return;
        }
        
        await _stocksContext.FinancialsYears.AddRangeAsync(newFinancials);
        await _stocksContext.SaveChangesAsync();

        await _serviceBus.Publish(
            new FinancialsYearUpdated(reports));
    }
}
