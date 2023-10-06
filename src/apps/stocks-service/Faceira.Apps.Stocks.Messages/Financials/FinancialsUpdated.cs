using System.ComponentModel.DataAnnotations;
using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages.Financials;

public class FinancialsUpdated : IMessage
{
    public FinancialsUpdated(IEnumerable<FinancialReport> financials)
    {
        Financials = financials;
    }

    public IEnumerable<FinancialReport> Financials { get; private set; }
}


public class FinancialReport
{
    public const string ReportTypeNominal = "nominal";
    public const string Growth1 = "growth-1";
    public const string Growth2 = "growth-2";
    public const string Growth3 = "growth-3";
    public const string Growth4 = "growth-4";
    public const string Growth5 = "growth-5";
    public const string Growth8 = "growth-8";
    public const string Growth10 = "growth-10";
    public const string Growth12 = "growth-12";

    public FinancialReport(string symbol, string type, int year, int quarter, DateTime periodStart, DateTime periodEnd, 
        decimal? revenue, decimal? costsOfGoodsSold, decimal? grossProfit, decimal? operatingExpenses, 
        decimal? operatingIncome, decimal? netIncome, decimal? ebitda, decimal? ebit, decimal? epsBasic, 
        decimal? epsDiluted, decimal? outstandingSharesBasic, decimal? outstandingSharesDiluted, 
        decimal? dividendPerShare, decimal? totalAssets, decimal? totalLiabilities, decimal? totalEquity, 
        decimal? debtTotal, decimal? debtNet, decimal? bookValuePerShare, decimal? cashFlowOperating, 
        decimal? cashFlowInvesting, decimal? cashFlowFinancing, decimal? cashFlowFree)
    {
        Symbol = symbol;
        Type = type;
        Year = year;
        Quarter = quarter;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
        
        Revenue = revenue;
        CostsOfGoodsSold = costsOfGoodsSold;
        GrossProfit = grossProfit;
        OperatingExpenses = operatingExpenses;
        OperatingIncome = operatingIncome;
        NetIncome = netIncome;
        Ebitda = ebitda;
        Ebit = ebit;
        EpsBasic = epsBasic;
        EpsDiluted = epsDiluted;
        OutstandingSharesBasic = outstandingSharesBasic;
        OutstandingSharesDiluted = outstandingSharesDiluted;
        DividendPerShare = dividendPerShare;
        
        TotalAssets = totalAssets;
        TotalLiabilities = totalLiabilities;
        TotalEquity = totalEquity;
        DebtTotal = debtTotal;
        DebtNet = debtNet;
        BookValuePerShare = bookValuePerShare;
        
        CashFlowOperating = cashFlowOperating;
        CashFlowInvesting = cashFlowInvesting;
        CashFlowFinancing = cashFlowFinancing;
        CashFlowFree = cashFlowFree;
    }

    [Key]
    [MaxLength(10)]
    public string Symbol { get; private set; }
    
    [Key]
    [MaxLength(10)]
    public string Type { get; private set; }

    [Key]
    public int Year { get; private set; }
    
    [Key]
    public int Quarter { get; private set; }
    
    public DateTime PeriodStart { get; private set; }
    
    public DateTime PeriodEnd { get; private set; }
    
    
    // Income Statement
    
    public decimal? Revenue { get; private set; }
    
    public decimal? CostsOfGoodsSold { get; private set; }
    
    public decimal? GrossProfit { get; private set; }
    
    public decimal? OperatingExpenses { get; private set; }
    
    public decimal? OperatingIncome { get; private set; }
    
    public decimal? NetIncome { get; private set; }
    
    public decimal? Ebitda { get; private set; }
    
    public decimal? Ebit { get; private set; }
    
    public decimal? EpsBasic { get; private set; }
    
    public decimal? EpsDiluted { get; private set; }
    
    public decimal? OutstandingSharesBasic { get; private set; }
    
    public decimal? OutstandingSharesDiluted { get; private set; }
    
    public decimal? DividendPerShare { get; private set; }
    
    
    // Balance Sheet
    
    public decimal? TotalAssets { get; private set; }
    
    public decimal? TotalLiabilities { get; private set; }
    
    public decimal? TotalEquity { get; private set; }
    
    public decimal? DebtTotal { get; private set; }
    
    public decimal? DebtNet { get; private set; }
    
    public decimal? BookValuePerShare { get; private set; }
    
    
    // Cash Flow 
    
    public decimal? CashFlowOperating { get; private set; }
    
    public decimal? CashFlowInvesting { get; private set; }
    
    public decimal? CashFlowFinancing { get; private set; }
    
    public decimal? CashFlowFree { get; private set; }

}