FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
COPY . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "ConnectUs.csproj" --configuration Release
RUN chmod +x docker/app/entrypoint.sh
CMD /bin/sh docker/app/entrypoint.sh
