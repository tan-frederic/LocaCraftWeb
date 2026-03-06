# Stage 1: Build
FROM node:20-alpine AS builder

WORKDIR /app

ARG API_URL=http://localhost:8080/api

COPY package*.json ./
RUN npm ci

COPY . .

RUN sed -i "s|__API_URL__|${API_URL}|g" src/app/environments/environment.prod.ts

RUN npm run build -- --configuration production

# Stage 2: Serve
FROM nginx:alpine

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=builder /app/dist/loca-craft-app/browser /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
