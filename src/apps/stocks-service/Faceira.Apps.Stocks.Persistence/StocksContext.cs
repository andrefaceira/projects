using Faceira.Apps.Stocks.Messages.Companies;
using Faceira.Apps.Stocks.Messages.Financials;
using Microsoft.EntityFrameworkCore;

namespace Faceira.Apps.Stocks.Persistence;

public class StocksContext : DbContext
{
    public StocksContext(DbContextOptions<StocksContext> builderOptions) 
        : base(builderOptions)
    {
        
    }
    
    public DbSet<CompanyUpdated> Companies { get; set; } = default!;
    public DbSet<FinancialReport> Financials { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stocks");

        modelBuilder.Entity<FinancialReport>().HasKey(p => new { p.Symbol, p.Type, p.Year, p.Quarter });
        modelBuilder.Entity<FinancialReport>().HasIndex(p => new { p.Symbol, p.Type });
    }
}