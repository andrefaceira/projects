using System.Text.Json;

namespace Faceira.Apps.Stocks.Application;

public interface IMapper<out T> 
    where T : class
{
    T Map(JsonElement response);
}