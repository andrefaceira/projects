using Faceira.Shared.Application.Messages;

namespace Faceira.Shared.Application.Application;

public interface IDispatcher
{
    Task Dispatch<TMessage>(TMessage message)
        where TMessage : IMessage;
}