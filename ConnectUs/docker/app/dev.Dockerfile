FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build
COPY . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "ConnectUs.csproj" --configuration Release
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
RUN chmod +x docker/app/dev-entrypoint.sh
CMD /bin/sh docker/app/dev-entrypoint.sh

