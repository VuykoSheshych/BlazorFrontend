FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Client/Frontend.Client.csproj", "Client/"]
RUN dotnet restore "Client/Frontend.Client.csproj"

COPY . .
WORKDIR "/src/Client"
RUN dotnet build "Frontend.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frontend.Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html

COPY --from=publish /app/publish/wwwroot .
COPY Client/nginx.conf /etc/nginx/nginx.conf