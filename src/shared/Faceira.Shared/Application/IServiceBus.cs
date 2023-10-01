using Faceira.Shared.Application.Messages;

namespace Faceira.Shared.Application.Application;

public interface IServiceBus
{
    Task Publish<T>(T message) 
        where T : IMessage;
}