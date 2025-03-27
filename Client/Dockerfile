FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Client/Frontend.Client.csproj Client/
RUN dotnet restore Client/Frontend.Client.csproj

COPY . .
WORKDIR /app/Client
RUN dotnet build Frontend.Client.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Frontend.Client.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM busybox:latest AS final
WORKDIR /wwwroot

COPY --from=publish /app/publish/wwwroot .

CMD ["httpd", "-f", "-p", "80"]
EXPOSE 80