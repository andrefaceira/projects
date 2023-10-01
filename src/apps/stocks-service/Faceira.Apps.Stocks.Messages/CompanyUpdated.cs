using System.ComponentModel.DataAnnotations;
using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages;

public class CompanyUpdated : IMessage
{
    public CompanyUpdated(string symbol, string name, string exchange, string currency, string country, DateTime ipoDate, string industry)
    {
        Symbol = symbol;
        CreatedAt = DateTime.UtcNow; 
        Name = name;
        Exchange = exchange;
        Currency = currency;
        Country = country;
        IpoDate = ipoDate;
        Industry = industry;
    }

    [Key]
    [MaxLength(10)]
    public string Symbol { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    [MaxLength(100)]
    public string Name { get; private set; }
    
    [MaxLength(100)]
    public string Exchange { get; private set; }
    
    [MaxLength(100)]
    public string Currency { get; private set; }
    
    [MaxLength(100)]
    public string Country { get; private set; } 
    
    public DateTime IpoDate { get; private set; }
    
    [MaxLength(100)]
    public string Industry { get; private set; }
}