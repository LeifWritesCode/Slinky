# build the project
FROM node:18-alpine as build-env
WORKDIR /app
COPY . ./
ENV NODE_ENV production
RUN npm install
#RUN npm audit fix --force
RUN npm run build

# serve separately
FROM node:18-alpine
WORKDIR /app
ENV NODE_ENV production
RUN npm install -g serve
COPY --from=build-env /app/build .
EXPOSE 3000
ENTRYPOINT ["serve", "-s", "."]
