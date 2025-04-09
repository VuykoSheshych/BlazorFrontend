FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY BlazorFrontend.csproj ./
RUN dotnet restore BlazorFrontend.csproj

COPY . . 
WORKDIR /app
RUN dotnet publish BlazorFrontend.csproj -c Release -o /app/publish \
	/p:BlazorEnvironment=${BLAZOR_ENVIRONMENT} \
	/p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf

ENTRYPOINT ["sh", "-c", "nginx -g 'daemon off;'"]

EXPOSE 80
EXPOSE 443