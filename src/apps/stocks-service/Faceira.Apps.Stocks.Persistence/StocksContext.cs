using Faceira.Apps.Stocks.Messages;
using Microsoft.EntityFrameworkCore;

namespace Faceira.Apps.Stocks.Persistence;

public class StocksContext : DbContext
{
    public StocksContext(DbContextOptions<StocksContext> builderOptions) 
        : base(builderOptions)
    {
        
    }

    public DbSet<CompanyUpdated> Companies { get; set; } = default!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("stocks");
        
    }
}