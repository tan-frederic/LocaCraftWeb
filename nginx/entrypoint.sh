#!/bin/sh
set -e

CERT="/etc/letsencrypt/live/api.locacraft.fr/fullchain.pem"
NGINX_CONF="/etc/nginx/conf.d/default.conf"
BOOTSTRAP_CONF="/etc/nginx/bootstrap.conf"

# ─────────────────────────────────────────────
# PHASE 1 : Démarrage en mode bootstrap (HTTP only)
# Nginx répond sur le port 80 pour que Certbot puisse faire son challenge
# ─────────────────────────────────────────────
if [ ! -f "$CERT" ]; then
    echo "[entrypoint] No certificate found — starting in bootstrap mode (HTTP only)..."
    cp "$BOOTSTRAP_CONF" "$NGINX_CONF"
    nginx -g "daemon on;"

    echo "[entrypoint] Waiting for Certbot to generate certificate..."
    i=0
    while [ ! -f "$CERT" ] && [ $i -lt 24 ]; do
        sleep 5
        i=$((i+1))
        echo "[entrypoint] Still waiting... ($((i*5))s)"
    done

    if [ ! -f "$CERT" ]; then
        echo "[entrypoint] ERROR: Certificate not found after 120s."
        echo "[entrypoint] Check Certbot logs: docker compose logs certbot"
        nginx -s stop
        exit 1
    fi

    echo "[entrypoint] Certificate found! Switching to full SSL config..."
    nginx -s stop
    sleep 2
fi

# ─────────────────────────────────────────────
# PHASE 2 : Démarrage normal avec SSL
# ─────────────────────────────────────────────
echo "[entrypoint] Starting Nginx with SSL (Let's Encrypt)..."
cp /etc/nginx/ssl.conf "$NGINX_CONF"
exec nginx -g "daemon off;"