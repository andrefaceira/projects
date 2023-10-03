using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages.Financials;

public class FinancialsQuarterUpdateTriggered : IMessage
{
    public FinancialsQuarterUpdateTriggered(string symbol)
    {
        Symbol = symbol;
    }

    public string Symbol { get; private set; }
}