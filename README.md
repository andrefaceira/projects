# projects

## Run local

    docker-compose up

## Migrations

    cd ./src/apps/stocks-service/Faceira.Apps.Stocks.Service

    dotnet ef migrations add AddCompaniesTable --project ../Faceira.Apps.Stocks.Persistence 

    dotnet ef database update --project ../Faceira.Apps.Stocks.Persistence 
