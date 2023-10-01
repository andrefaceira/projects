using Faceira.Shared.Application;
using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages;

public class CompanyUpdateTriggered : IMessage
{
    public CompanyUpdateTriggered(string symbol)
    {
        Symbol = symbol;
    }

    public string Symbol { get; private set; }
}