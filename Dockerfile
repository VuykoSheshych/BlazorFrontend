FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY ./publish/wwwroot . 
COPY nginx.conf /etc/nginx/nginx.conf

ENTRYPOINT ["sh", "-c", "nginx -g 'daemon off;'"]

EXPOSE 80
EXPOSE 443