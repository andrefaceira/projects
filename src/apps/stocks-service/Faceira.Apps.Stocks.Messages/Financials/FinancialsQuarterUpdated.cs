using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages.Financials;

public class FinancialsQuarterUpdated : IMessage
{
    public FinancialsQuarterUpdated(IEnumerable<ReportUpdated> reports)
    {
        Reports = reports;
    }

    public IEnumerable<ReportUpdated> Reports { get; private set; }
}