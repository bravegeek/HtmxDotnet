# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /source

# copy csproj and restore as distinct layers
COPY App/*.csproj .
RUN dotnet restore -a $TARGETARCH

# copy and publish app and libraries
COPY App/. .
RUN dotnet publish -a $TARGETARCH --no-restore -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./HtmxDotnet"]


#
# how to use this:

# build image using dockerfile
#> docker build -t htmx-image -f Dockerfile .

# run the image and listen on localhost:8384
#> docker run -it --rm -p 8384:8080 --name htmx-container htmx-image


# explicitly create container from the image (this is clunky)
#> docker create --name htmx-container htmx-image
