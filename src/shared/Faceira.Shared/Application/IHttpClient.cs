namespace Faceira.Shared.Application.Application;

public interface IHttpClient
{
    Task<T> Get<T>(string path);
}