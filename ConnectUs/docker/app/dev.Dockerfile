FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
COPY . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "ConnectUs.csproj" --configuration Release
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENTRYPOINT dotnet watch run --server.urls http://*:5000


