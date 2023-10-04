
ARG NET_IMAGE=8.0

FROM mcr.microsoft.com/dotnet/aspnet:${NET_IMAGE} AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:${NET_IMAGE} AS build
WORKDIR /src
COPY ["apps/stocks-service/Faceira.Apps.Stocks.Service/Faceira.Apps.Stocks.Service.csproj", "apps/stocks-service/Faceira.Apps.Stocks.Service/"]
COPY ["shared/Faceira.Shared/Faceira.Shared.csproj", "shared/Faceira.Shared/"]
RUN dotnet restore "apps/stocks-service/Faceira.Apps.Stocks.Service/Faceira.Apps.Stocks.Service.csproj"
COPY . .
WORKDIR "/src/apps/stocks-service/Faceira.Apps.Stocks.Service"
RUN dotnet build "Faceira.Apps.Stocks.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Faceira.Apps.Stocks.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Faceira.Apps.Stocks.Service.dll"]
