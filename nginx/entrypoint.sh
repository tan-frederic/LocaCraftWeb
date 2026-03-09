#!/bin/sh
set -e

CERT="/etc/letsencrypt/live/api.locacraft.fr/fullchain.pem"
KEY="/etc/letsencrypt/live/api.locacraft.fr/privkey.pem"

if [ ! -f "$CERT" ]; then
    echo "No SSL certificate found — generating self-signed certificate..."
    mkdir -p /etc/letsencrypt/live/api.locacraft.fr
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
        -keyout "$KEY" \
        -out "$CERT" \
        -subj "/CN=locacraft.fr"
    echo "Self-signed certificate generated."
fi

exec nginx -g "daemon off;"
