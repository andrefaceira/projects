using System.Collections.ObjectModel;
using Dapr.Client;

namespace Faceira.Shared.Application.Application.HttpClients;

public class DaprHttpClient : IHttpClient
{
    private readonly DaprClient _daprClient;
    private readonly string _bindingName;

    public DaprHttpClient(DaprClient daprClient, string bindingName)
    {
        _daprClient = daprClient;
        _bindingName = bindingName;
    }

    public async Task<T> Get<T>(string path)
    {
        var response = await _daprClient.InvokeBindingAsync<string, string>(
            _bindingName, 
            "get", 
            string.Empty, 
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
            {
                { "path", path }
            }));
        Console.WriteLine(response);
        throw new Exception();
        
        // return response;
    }
}