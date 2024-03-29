using System.Globalization;
using System.Text.Json;
using Faceira.Apps.Stocks.Messages.Financials;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers.Financials.Mappers;

public class FinancialsMapper : IMapper<IEnumerable<FinancialReport>>
{ 
    public IEnumerable<FinancialReport> Map(JsonElement response)
    {
        return response.GetProperty("data")
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
                Report = p.GetProperty("report").GetProperty("bs").EnumerateArray()
                    .Concat(p.GetProperty("report").GetProperty("ic").EnumerateArray())
                    .Concat(p.GetProperty("report").GetProperty("cf").EnumerateArray())
                    .Select(pp => new KeyValuePair<string, decimal?>(
                        pp.GetProperty("concept").ToString(),
                        pp.GetProperty("value").GetDecimal()
                    ))
            })
            .Select(p => new FinancialReport(
                p.Symbol,
                FinancialReport.ReportTypeNominal,
                p.Year,
                p.Quarter,
                p.PeriodStart,
                p.PeriodEnd,
                
                MapConceptMillions(p.Report, "us-gaap_RevenueFromContractWithCustomerExcludingAssessedTax"),
                MapConceptMillions(p.Report, "us-gaap_CostOfGoodsAndServicesSold"),
                MapConceptMillions(p.Report, "us-gaap_GrossProfit"),
                MapConceptMillions(p.Report, "us-gaap_OperatingExpenses"),
                MapConceptMillions(p.Report, "us-gaap_OperatingIncomeLoss"),
                MapConceptMillions(p.Report, "us-gaap_NetIncomeLoss"),
                0, // ebitda
                0, // ebit
                MapConcept(p.Report, "us-gaap_EarningsPerShareBasic"),
                MapConcept(p.Report, "us-gaap_EarningsPerShareDiluted"),
                MapConceptMillions(p.Report, "us-gaap_WeightedAverageNumberOfSharesOutstandingBasic"),
                MapConceptMillions(p.Report, "us-gaap_WeightedAverageNumberOfDilutedSharesOutstanding"),
                0, // dividends per share
                
                MapConceptMillions(p.Report, "us-gaap_Assets"),
                MapConceptMillions(p.Report, "us-gaap_Liabilities"),
                MapConceptMillions(p.Report, "us-gaap_StockholdersEquity"),
                0, // DebtTotal
                0, // DebtNet
                0, // book value per share
                
                MapConceptMillions(p.Report, "us-gaap_NetCashProvidedByUsedInOperatingActivities"),
                MapConceptMillions(p.Report, "us-gaap_NetCashProvidedByUsedInInvestingActivities"),
                MapConceptMillions(p.Report, "us-gaap_NetCashProvidedByUsedInFinancingActivities"),
                0 // free cash flow
            ));
    }

    private decimal? MapConcept(IEnumerable<KeyValuePair<string, decimal?>> report, string concept)
    {
        return report
            .Where(p => p.Key == concept)
            .Select(p => p.Value)
            .FirstOrDefault();
    }
    
    private decimal? MapConceptMillions(IEnumerable<KeyValuePair<string, decimal?>> report, 
        string concept, int? roundPrecision = null)
    {
        var value = MapConcept(report, concept);
        if (value == null)
        {
            return null;
        }
        
        return decimal.Round(
            value.Value / 1000000, 
            roundPrecision ?? 2);
    }
}