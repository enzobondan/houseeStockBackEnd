FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY api-stock.csproj .
RUN dotnet restore api-stock.csproj

COPY . .
RUN dotnet publish api-stock.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080

COPY --from=build /app ./

ENTRYPOINT ["dotnet", "api-stock.dll"]