using Faceira.Shared.Application.Messages;

namespace Faceira.Shared.Application.Application;

public interface IHandle<in TMessage> 
    where TMessage : IMessage
{
    Task Handle(TMessage message);
}