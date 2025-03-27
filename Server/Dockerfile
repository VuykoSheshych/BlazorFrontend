FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Server/Frontend.Server.csproj", "Server/"]
RUN dotnet restore "Server/Frontend.Server.csproj"

COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Frontend.Server.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Frontend.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Frontend.Server.dll"]