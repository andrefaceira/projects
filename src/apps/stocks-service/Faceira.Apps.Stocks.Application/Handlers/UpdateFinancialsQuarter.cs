using Faceira.Apps.Stocks.Messages.Financials;
using Faceira.Shared.Application.Application;

namespace Faceira.Apps.Stocks.Application.Handlers;

public class UpdateFinancialsQuarter : IHandle<FinancialsQuarterUpdateTriggered>
{
    public Task Handle(FinancialsQuarterUpdateTriggered message)
    {
        throw new NotImplementedException();
    }
}