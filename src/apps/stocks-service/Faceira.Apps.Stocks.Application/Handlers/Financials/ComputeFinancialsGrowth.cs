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
        var reports = message.Financials
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Quarter);

        var periods = Enumerable.Range(1, 12);
        foreach (var period in periods)
        {
            await HandleFinancialsGrowthPeriod(reports, period);
        }
    }

    private async Task HandleFinancialsGrowthPeriod(IOrderedEnumerable<FinancialReport> reports, int periodsNumber)
    {
        var reportType = $"growth-{periodsNumber}";
        var computedReports = CalculateFinancialsGrowth(reports, reportType, periodsNumber);

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

    private IEnumerable<FinancialReport> CalculateFinancialsGrowth(IEnumerable<FinancialReport> reports, 
        string reportType, int periodsNumber)
    {
        var computedReports = new List<FinancialReport>();
        for (var i = 0; i < reports.Count() - periodsNumber; i++)
        {
            var currentReport = reports.ElementAt(i);
            var previousReport = reports.ElementAt(i + periodsNumber);

            computedReports.Add(new FinancialReport
            (
                currentReport.Symbol,
                reportType,
                currentReport.Year,
                currentReport.Quarter,
                currentReport.PeriodStart,
                currentReport.PeriodEnd,
                
                CalculateAverageGrowth(currentReport.Revenue, previousReport.Revenue, periodsNumber),
                CalculateAverageGrowth(currentReport.CostsOfGoodsSold, previousReport.CostsOfGoodsSold, periodsNumber),
                CalculateAverageGrowth(currentReport.GrossProfit, previousReport.GrossProfit, periodsNumber),
                CalculateAverageGrowth(currentReport.OperatingExpenses, previousReport.OperatingExpenses, periodsNumber),
                CalculateAverageGrowth(currentReport.OperatingIncome, previousReport.OperatingIncome, periodsNumber),
                CalculateAverageGrowth(currentReport.NetIncome, previousReport.NetIncome, periodsNumber),
                CalculateAverageGrowth(currentReport.Ebitda, previousReport.Ebitda, periodsNumber),
                CalculateAverageGrowth(currentReport.Ebit, previousReport.Ebit, periodsNumber),
                CalculateAverageGrowth(currentReport.EpsBasic, previousReport.EpsBasic, periodsNumber),
                CalculateAverageGrowth(currentReport.EpsDiluted, previousReport.EpsDiluted, periodsNumber),
                CalculateAverageGrowth(currentReport.OutstandingSharesBasic, previousReport.OutstandingSharesBasic, periodsNumber),
                CalculateAverageGrowth(currentReport.OutstandingSharesDiluted, previousReport.OutstandingSharesDiluted, periodsNumber),
                CalculateAverageGrowth(currentReport.DividendPerShare, previousReport.DividendPerShare, periodsNumber),
                CalculateAverageGrowth(currentReport.TotalAssets, previousReport.TotalAssets, periodsNumber),
                CalculateAverageGrowth(currentReport.TotalLiabilities, previousReport.TotalLiabilities, periodsNumber),
                CalculateAverageGrowth(currentReport.TotalEquity, previousReport.TotalEquity, periodsNumber),
                CalculateAverageGrowth(currentReport.DebtTotal, previousReport.DebtTotal, periodsNumber),
                CalculateAverageGrowth(currentReport.DebtNet, previousReport.DebtNet, periodsNumber),
                CalculateAverageGrowth(currentReport.BookValuePerShare, previousReport.BookValuePerShare, periodsNumber),
                CalculateAverageGrowth(currentReport.CashFlowOperating, previousReport.CashFlowOperating, periodsNumber),
                CalculateAverageGrowth(currentReport.CashFlowInvesting, previousReport.CashFlowInvesting, periodsNumber),
                CalculateAverageGrowth(currentReport.CashFlowFinancing, previousReport.CashFlowFinancing, periodsNumber),
                CalculateAverageGrowth(currentReport.CashFlowFree, previousReport.CashFlowFree, periodsNumber)
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