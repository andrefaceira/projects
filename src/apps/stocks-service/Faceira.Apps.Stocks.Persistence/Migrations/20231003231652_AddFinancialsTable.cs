using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Faceira.Apps.Stocks.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Financials",
                schema: "stocks",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Quarter = table.Column<int>(type: "integer", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revenue = table.Column<decimal>(type: "numeric", nullable: true),
                    CostsOfGoodsSold = table.Column<decimal>(type: "numeric", nullable: true),
                    GrossProfit = table.Column<decimal>(type: "numeric", nullable: true),
                    OperatingExpenses = table.Column<decimal>(type: "numeric", nullable: true),
                    OperatingIncome = table.Column<decimal>(type: "numeric", nullable: true),
                    NetIncome = table.Column<decimal>(type: "numeric", nullable: true),
                    Ebitda = table.Column<decimal>(type: "numeric", nullable: true),
                    Ebit = table.Column<decimal>(type: "numeric", nullable: true),
                    EpsBasic = table.Column<decimal>(type: "numeric", nullable: true),
                    EpsDiluted = table.Column<decimal>(type: "numeric", nullable: true),
                    OutstandingSharesBasic = table.Column<decimal>(type: "numeric", nullable: true),
                    OutstandingSharesDiluted = table.Column<decimal>(type: "numeric", nullable: true),
                    DividendPerShare = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalAssets = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalLiabilities = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalEquity = table.Column<decimal>(type: "numeric", nullable: true),
                    DebtTotal = table.Column<decimal>(type: "numeric", nullable: true),
                    DebtNet = table.Column<decimal>(type: "numeric", nullable: true),
                    BookValuePerShare = table.Column<decimal>(type: "numeric", nullable: true),
                    CashFlowOperating = table.Column<decimal>(type: "numeric", nullable: true),
                    CashFlowInvesting = table.Column<decimal>(type: "numeric", nullable: true),
                    CashFlowFinancing = table.Column<decimal>(type: "numeric", nullable: true),
                    CashFlowFree = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Financials", x => new { x.Symbol, x.Type, x.Year, x.Quarter });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Financials_Symbol_Type",
                schema: "stocks",
                table: "Financials",
                columns: new[] { "Symbol", "Type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Financials",
                schema: "stocks");
        }
    }
}
