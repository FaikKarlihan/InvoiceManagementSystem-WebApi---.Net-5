# Build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
