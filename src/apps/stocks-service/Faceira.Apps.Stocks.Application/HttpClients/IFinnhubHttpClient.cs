using System.Net.Http.Json;

namespace Faceira.Apps.Stocks.Application.HttpClients;

public interface IFinnhubHttpClient
{
    Task<T> Get<T>(string path);
}

public class FinnhubHttpClient : IFinnhubHttpClient
{
    private readonly HttpClient _httpClient;

    public FinnhubHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> Get<T>(string path)
    {
        var response = await _httpClient.GetFromJsonAsync<T>(path);
        if (response is null)
        {
            throw new InvalidOperationException($"FinnhubHttpClient.Get<{typeof(T).Name}> returned null");
        }

        return response;
    }
}