FROM node:20-alpine AS build
WORKDIR /app

COPY Repos-tutorat/package.json Repos-tutorat/package-lock.json ./
RUN npm install

COPY Repos-tutorat/ ./
RUN npm run build -- --configuration production

FROM nginx:alpine
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist/etude-reussie-angular-ui /usr/share/nginx/html

EXPOSE 80