using Faceira.Shared.Application.Service.Configuration;

namespace Faceira.Apps.Stocks.Application;

public class StocksApplicationConfiguration 
{
    public required DatabasesConfiguration Databases { get; init; }
    public required ServiceBusesConfiguration ServiceBuses { get; init; }
    public required ApisConfiguration Apis { get; init; }
}

public class DatabasesConfiguration
{
    public required DatabaseConfiguration StocksDatabase { get; init; }
}

public class ServiceBusesConfiguration
{
    public required ServiceBusConfiguration StocksServiceBus { get; init; }
}

public class ApisConfiguration
{
    public required string FinnhubApiBinding { get; init; }
}
