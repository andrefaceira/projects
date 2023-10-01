FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["apps/stocks/Faceira.Apps.Stocks.Api/Faceira.Apps.Stocks.Api.csproj", "apps/stocks/Faceira.Apps.Stocks.Api/"]
RUN dotnet restore "apps/stocks/Faceira.Apps.Stocks.Api/Faceira.Apps.Stocks.Api.csproj"
COPY . .
WORKDIR "/src/apps/stocks/Faceira.Apps.Stocks.Api"
RUN dotnet build "Faceira.Apps.Stocks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Faceira.Apps.Stocks.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faceira.Apps.Stocks.Api.dll"]
