FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
RUN cd Connect-Us/ConnectUs && cp -R . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN cd ../.. && dotnet publish "ConnectUs.csproj" --configuration Release
RUN chmod +x entrypoint.sh
CMD /bin/sh entrypoint.sh
