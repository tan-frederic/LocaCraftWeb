#!/bin/sh
set -e

CERT="/etc/letsencrypt/live/api.locacraft.fr/fullchain.pem"

if [ -f "$CERT" ]; then
    echo "[entrypoint] Certificate found — starting with SSL."
    cp /etc/nginx/ssl.conf /etc/nginx/conf.d/default.conf
else
    echo "[entrypoint] No certificate — bootstrap mode (HTTP only)."
    cp /etc/nginx/bootstrap.conf /etc/nginx/conf.d/default.conf
fi

exec nginx -g "daemon off;"