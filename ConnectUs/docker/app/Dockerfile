FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
RUN cd ../..
RUN cp -R . /app
WORKDIR /app
RUN cd app
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet publish "ConnectUs.csproj" --configuration Release
RUN chmod +x docker/app/entrypoint.sh
CMD /bin/sh docker/app/entrypoint.sh
