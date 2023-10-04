using System.Text.Json;

namespace Faceira.Shared.Application.Application;

public interface IMapper<out T> 
    where T : class
{
    T Map(JsonElement response);
}