using Faceira.Apps.Stocks.Messages.Financials;
using Faceira.Apps.Stocks.Persistence;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers.Financials;

public class ComputeFinancialsGrowth : IHandle<FinancialsUpdated>
{
    private readonly StocksContext _stocksContext;
    private readonly IServiceBus _serviceBus;

    public ComputeFinancialsGrowth(StocksContext stocksContext, IServiceBus serviceBus)
    {
        _stocksContext = stocksContext;
        _serviceBus = serviceBus;
    }

    public async Task Handle(FinancialsUpdated message)
    {
        var reports = message.Financials.OrderByDescending(p => p.Year);
        var numberOfReports = 1;
        var reportType = "TODO";

        
        // TODO: foreach reportType
        var computedReports = GetFinancialsGrowth(reports, reportType,  numberOfReports);
        
        var lastFinancial = _stocksContext.Financials
            .Where(p => p.Symbol == reports.First().Symbol)
            .Where(p => p.Type == reportType)
            .Where(p => (reports.First().Quarter == 0 && p.Quarter == 0) ||
                        (reports.First().Quarter > 0 && p.Quarter > 0))
            .OrderBy(p => p.Year)
            .ThenBy(p => p.Quarter)
            .FirstOrDefault();
        
        var newFinancials = computedReports
            .Where(p => p.Year >= lastFinancial?.Year || 
                        (p.Year == lastFinancial?.Year && p.Quarter > lastFinancial.Quarter));

        await _stocksContext.AddRangeAsync(newFinancials);
        
        await _serviceBus.Publish(
            new FinancialsUpdated(computedReports));
        
        
        
        
        await _stocksContext.SaveChangesAsync();
    }

    private IEnumerable<FinancialReport> GetFinancialsGrowth(IOrderedEnumerable<FinancialReport> reports, 
        string reportType, int numberOfReports)
    {
        var computedReports = new List<FinancialReport>();
        for (var i = 0; i < reports.Count() - numberOfReports; i++)
        {
            var currentReport = reports.ElementAt(i);
            var previousReport = reports.ElementAt(i + numberOfReports);

            computedReports.Add(new FinancialReport
            (
                currentReport.Symbol,
                reportType,
                currentReport.Year,
                currentReport.Quarter,
                currentReport.PeriodStart,
                currentReport.PeriodEnd,
                
                CalculateAverageGrowth(currentReport.Revenue, previousReport.Revenue, numberOfReports),
                CalculateAverageGrowth(currentReport.CostsOfGoodsSold, previousReport.CostsOfGoodsSold, numberOfReports),
                CalculateAverageGrowth(currentReport.GrossProfit, previousReport.GrossProfit, numberOfReports),
                CalculateAverageGrowth(currentReport.OperatingExpenses, previousReport.OperatingExpenses, numberOfReports),
                CalculateAverageGrowth(currentReport.OperatingIncome, previousReport.OperatingIncome, numberOfReports),
                CalculateAverageGrowth(currentReport.NetIncome, previousReport.NetIncome, numberOfReports),
                CalculateAverageGrowth(currentReport.Ebitda, previousReport.Ebitda, numberOfReports),
                CalculateAverageGrowth(currentReport.Ebit, previousReport.Ebit, numberOfReports),
                CalculateAverageGrowth(currentReport.EpsBasic, previousReport.EpsBasic, numberOfReports),
                CalculateAverageGrowth(currentReport.EpsDiluted, previousReport.EpsDiluted, numberOfReports),
                CalculateAverageGrowth(currentReport.OutstandingSharesBasic, previousReport.OutstandingSharesBasic, numberOfReports),
                CalculateAverageGrowth(currentReport.OutstandingSharesDiluted, previousReport.OutstandingSharesDiluted, numberOfReports),
                CalculateAverageGrowth(currentReport.DividendPerShare, previousReport.DividendPerShare, numberOfReports),
                CalculateAverageGrowth(currentReport.TotalAssets, previousReport.TotalAssets, numberOfReports),
                CalculateAverageGrowth(currentReport.TotalLiabilities, previousReport.TotalLiabilities, numberOfReports),
                CalculateAverageGrowth(currentReport.TotalEquity, previousReport.TotalEquity, numberOfReports),
                CalculateAverageGrowth(currentReport.DebtTotal, previousReport.DebtTotal, numberOfReports),
                CalculateAverageGrowth(currentReport.DebtNet, previousReport.DebtNet, numberOfReports),
                CalculateAverageGrowth(currentReport.BookValuePerShare, previousReport.BookValuePerShare, numberOfReports),
                CalculateAverageGrowth(currentReport.CashFlowOperating, previousReport.CashFlowOperating, numberOfReports),
                CalculateAverageGrowth(currentReport.CashFlowInvesting, previousReport.CashFlowInvesting, numberOfReports),
                CalculateAverageGrowth(currentReport.CashFlowFinancing, previousReport.CashFlowFinancing, numberOfReports),
                CalculateAverageGrowth(currentReport.CashFlowFree, previousReport.CashFlowFree, numberOfReports)
            ));
        }

        return computedReports;
    }

    private decimal? CalculateAverageGrowth(decimal? currentValue, decimal? previousValue, int? periodsNumber = 1)
    {
        if (previousValue is null or 0 ||
            currentValue is null or 0)
        {
            return null;
        }

        return Math.Round(
            Convert.ToDecimal(
                Math.Pow(
                    Convert.ToDouble(
                        currentValue / previousValue),
                    Convert.ToDouble(
                        1 / periodsNumber)) 
                - 1)
            * 100,
            2);
    }
}