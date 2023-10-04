using Faceira.Apps.Stocks.Application;
using Faceira.Shared.Application;
using Faceira.Shared.Application.Service.Configuration;


var builder = DefaultApplicationBuilder.CreateApiBuilder(args);


builder.Services.AddStocksApplication(new StocksApplicationConfiguration
{
    // TODO: may be improved?
    Databases = new DatabasesConfiguration
    {
        StocksDatabase = new DatabaseConfiguration
        {
            ConnectionString = builder.Configuration["Databases:StocksDatabase:ConnectionString"]
                               ?? throw new InvalidOperationException("Databases:StocksDatabase:ConnectionString is not configured"),
            DefaultSchema = builder.Configuration["Databases:StocksDatabase:DefaultSchema"]
                            ?? throw new InvalidOperationException("Databases:StocksDatabase:DefaultSchema is not configured")
        }
    },
    ServiceBuses = new ServiceBusesConfiguration
    {
        StocksServiceBus = new ServiceBusConfiguration
        {
            Name = builder.Configuration["ServiceBuses:StocksServiceBus:Name"]
                   ?? throw new InvalidOperationException("ServiceBuses:StocksServiceBus:Name is not configured")
        }
    },
    Apis = new ApisConfiguration
    {
        FinnhubApiBinding = builder.Configuration["Apis:FinnhubApiBinding"]
                            ?? throw new InvalidOperationException("Apis:FinnhubApiBinding is not configured")
    }
});

var api = builder.BuildApi(
    typeof(StocksApplicationInstaller).Assembly);

api.Run();
