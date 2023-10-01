using System.ComponentModel.DataAnnotations;
using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages;

public class FinancialsUpdated : IMessage
{
    public FinancialsUpdated(string symbol, int year, int quarter, DateTime periodStart, DateTime periodEnd)
    {
        Symbol = symbol;
        Year = year;
        Quarter = quarter;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    [Key]
    [MaxLength(10)]
    public string Symbol { get; private set; }
    
    public int Year { get; private set; }
    
    public int Quarter { get; private set; }
    
    public DateTime PeriodStart { get; private set; }
    
    public DateTime PeriodEnd { get; private set; }
    
}