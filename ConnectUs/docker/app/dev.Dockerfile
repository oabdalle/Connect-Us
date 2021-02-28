FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build
COPY . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "ConnectUs.csproj" --configuration Release
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
RUN apk add dos2unix && apk add --no-cache bash
RUN dos2unix docker/app/dev-entrypoint.sh
RUN sed -i 's/\r$//' docker/app/dev-entrypoint.sh && chmod +x docker/app/dev-entrypoint.sh
ENTRYPOINT ["/bin/bash", "docker/app/dev-entrypoint.sh"]


