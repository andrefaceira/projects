namespace Faceira.Shared.Application.Service.Configuration;

public class ApiConfiguration
{
    public required string Url { get; init; }
    public Dictionary<string, string>? HttpHeaders { get; init; }
}