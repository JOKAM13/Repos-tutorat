FROM node:20-alpine AS build
WORKDIR /app

COPY Repos-tutorat/package.json Repos-tutorat/package-lock.json ./
RUN npm install

COPY Repos-tutorat/ ./
RUN npm run build -- --configuration production

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80