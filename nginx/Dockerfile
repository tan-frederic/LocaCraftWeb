FROM nginx:alpine

# Copie les deux configs
COPY nginx.bootstrap.conf /etc/nginx/bootstrap.conf
COPY nginx.conf           /etc/nginx/ssl.conf

# Config active au démarrage (sera remplacée par entrypoint.sh)
COPY nginx.bootstrap.conf /etc/nginx/conf.d/default.conf

# Entrypoint
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 80 443

ENTRYPOINT ["/entrypoint.sh"]