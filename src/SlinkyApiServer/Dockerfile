FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app

# Copy the project and build
COPY . ./
RUN dotnet publish -c Release -o out Slinky.sln

# Pull asp.net core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app

# Copy in build output
COPY --from=build-env /app/out .

# Start the bootstrapper on run
ENTRYPOINT ["dotnet", "Slinky.Api.dll"]
