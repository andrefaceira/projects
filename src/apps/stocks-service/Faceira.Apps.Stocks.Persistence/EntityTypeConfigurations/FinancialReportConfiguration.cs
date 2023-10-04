using Faceira.Apps.Stocks.Messages.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Faceira.Apps.Stocks.Persistence.EntityTypeConfigurations;

public class FinancialReportConfiguration : IEntityTypeConfiguration<FinancialReport>
{
    public void Configure(EntityTypeBuilder<FinancialReport> builder)
    {
        builder.HasKey(p => new { p.Symbol, p.Type, p.Year, p.Quarter });
        builder.HasIndex(p => new { p.Symbol, p.Type });
    }
}