using Faceira.Apps.Stocks.Application;
using Faceira.Shared.Application;
using Faceira.Shared.Application.Service.Configuration;


var builder = DefaultApplicationBuilder.CreateApiBuilder(args);

builder.Services.AddHandlersAssemblies(
    typeof(StocksApplicationInstaller).Assembly);

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
        FinnhubApi = new ApiConfiguration
        {
            Url = builder.Configuration["Apis:FinnhubApi:Url"]
                  ?? throw new InvalidOperationException("Apis:FinnhubApi:Url is not configured"),
            HttpHeaders = new Dictionary<string, string>
            {
                {
                    builder.Configuration["Apis:FinnhubApi:HttpHeaderAuthKey"]
                    ?? throw new InvalidOperationException("Apis:FinnhubApi:HttpHeaderAuthKey is not configured"),
                    builder.Configuration["Apis:FinnhubApi:HttpHeaderAuthValue"]
                    ?? throw new InvalidOperationException("Apis:FinnhubApi:HttpHeaderAuthValue is not configured")
                }
            }
        },
        ZacksRankApi = new ApiConfiguration
        {
            Url = builder.Configuration["Apis:ZacksRankApi:Url"]
                  ?? throw new InvalidOperationException("Apis:ZacksRankApi:Url is not configured"),
        }
    }
});

var api = builder.BuildApi();
api.BuildRoutes(
    typeof(StocksApplicationInstaller).Assembly);

api.Run();
