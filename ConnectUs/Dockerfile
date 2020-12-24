FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . /app
WORKDIR /app/ConnectUs
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet restore "ConnectUs.csproj"
RUN dotnet build "ConnectUs.csproj" -c Release
EXPOSE 80/tcp
RUN chmod +x entrypoint.sh
CMD /bin/bash entrypoint.sh
