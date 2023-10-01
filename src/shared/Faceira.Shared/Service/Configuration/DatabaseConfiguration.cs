namespace Faceira.Shared.Application.Service.Configuration;

public class DatabaseConfiguration
{
    public required string ConnectionString { get; init; }
    public required string DefaultSchema { get; init; }
}