FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Client/Frontend.Client.csproj Client/
RUN dotnet restore Client/Frontend.Client.csproj

COPY . . 
WORKDIR /app/Client
RUN dotnet publish Frontend.Client.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .
COPY Client/nginx.conf /etc/nginx/nginx.conf

ENTRYPOINT ["/bin/sh", "-c", "echo '{\"BLAZOR_SERVER_URL\": \"'$BLAZOR_SERVER_URL'\", \"GAMEPLAY_URL\": \"'$GAMEPLAY_URL'\"}' > /usr/share/nginx/html/blazor-config.json && exec nginx -g 'daemon off;'"]

EXPOSE 80
EXPOSE 443