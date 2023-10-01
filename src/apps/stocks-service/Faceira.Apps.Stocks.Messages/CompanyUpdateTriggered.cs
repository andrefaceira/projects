using Faceira.Shared.Application;
using Faceira.Shared.Application.Messages;

namespace Faceira.Apps.Stocks.Messages;

public record CompanyUpdateTriggered(
    string Symbol
) : IMessage;