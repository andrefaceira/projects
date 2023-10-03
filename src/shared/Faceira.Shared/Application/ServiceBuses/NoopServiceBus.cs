using Faceira.Shared.Application.Messages;

namespace Faceira.Shared.Application.Application.ServiceBuses;

public class NoopServiceBus : IServiceBus
{
    public Task Publish<T>(T message) where T : IMessage
    {
        return Task.CompletedTask;
    }
}