using Faceira.Shared.Application.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Faceira.Shared.Application.Application.Dispatchers;

public class DefaultDispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Dispatch<TMessage>(TMessage message) 
        where TMessage : IMessage
    {
        var handlers = _serviceProvider.GetServices<IHandle<TMessage>>();
        
        if (!handlers.Any())
        {
            throw new InvalidOperationException($"No handlers found for message {typeof(TMessage).Name}");
        }
        
        await Task.WhenAll(
            handlers.Select(p => p.Handle(message)));
    }
}