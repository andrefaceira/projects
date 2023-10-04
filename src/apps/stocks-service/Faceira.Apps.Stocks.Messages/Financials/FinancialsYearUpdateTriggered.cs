using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages.Financials;

public class FinancialsYearUpdateTriggered : IMessage
{
    public FinancialsYearUpdateTriggered(string symbol)
    {
        Symbol = symbol;
    }

    public string Symbol { get; private set; }
}