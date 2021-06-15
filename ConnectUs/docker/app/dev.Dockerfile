FROM mcr.microsoft.com/dotnet/sdk:5.0-focal as build
COPY . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /app/ConnectUs
RUN dotnet publish "ConnectUs.csproj" --configuration Release
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
RUN apt-get update && apt-get install dos2unix \
        && apt-get install -y --no-install-recommends unzip \
        && rm -rf /var/lib/apt/lists/* \
        && curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg
RUN chmod +x docker/app/dev-entrypoint.sh
RUN dos2unix docker/app/dev-entrypoint.sh
ENTRYPOINT ["/bin/bash", "docker/app/dev-entrypoint.sh"]


