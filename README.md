# Slinky

Slinky is a shortlinking app built using React and ASP.NET Core 6.

It aims to demonstrate the principles involved in designing a distributed, scaleable web-application characterised by high-availability.

The application is open source, and intended to be used as a learning tool. Please feel free to hack around it as you see fit.

You learn more about Slinky [on my blog](https://leif.uk/tag/slinky).

## Prerequisites

The following applications are required to deploy Slinky:

- Docker, supporting Compose v3

In order to develop and run Slinky on a host machine, the following dependencies should also be met:

- ASP.NET Core 6 SDK
- Node.JS
- Your editor of choice

## Deploying Slinky using Docker

```
docker compose -f docker/docker-compose.yml up
```

If you have not previously built the Slinky API and UX container images, they will be built during the deployment process.

The images can be built independently of compose using the following commands:

```
# to build the api server
docker build -f src/SlinkApiServer/Dockerfile -t lwg/slinky:dev src/SlinkyApiServer

# to build the front end image
docker build -f src/SlinkyFrontEnd/Dockerfile -t lwg/slinkyux:dev src/SlinkyFrontEnd
```

## Deploying Slinky on Bare Metal

Building the two components can be achieved with `dotnet publish` and `npm run build` in the `src/SlinkyApiServer` and `src/SlinkyFrontEnd` directories respectively.

You can then run the executable output of the former (`Slinky.Api.exe`) and host the content of the latter (in the `build` directory) using your favourite HTTP server or `serve -s build`.
