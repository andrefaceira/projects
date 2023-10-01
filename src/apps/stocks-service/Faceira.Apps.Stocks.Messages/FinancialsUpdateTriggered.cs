using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages;

public class FinancialsUpdateTriggered : IMessage
{
    public FinancialsUpdateTriggered(string symbol)
    {
        Symbol = symbol;
    }

    public string Symbol { get; private set; }
}