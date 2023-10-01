using Dapr.Client;
using Faceira.Shared.Application.Application;
using Faceira.Shared.Application.Messages;
using Microsoft.Extensions.Options;

namespace Faceira.Shared.Application.Dapr;

public class DaprServiceBus : IServiceBus
{
    private readonly DaprClient _daprClient;
    private readonly string _bindingName;

    public DaprServiceBus(DaprClient daprClient, string bindingName)
    {
        _daprClient = daprClient;
        _bindingName = bindingName;
    }

    public async Task Publish<T>(T message) where T : IMessage
    {
        await _daprClient.PublishEventAsync(
            pubsubName: _bindingName, 
            topicName: typeof(T).FullName, 
            message);
    }
}
