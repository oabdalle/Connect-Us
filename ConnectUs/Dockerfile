FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ConnectUs/ConnectUs.csproj", "ConnectUs/"]
RUN dotnet restore "ConnectUs/ConnectUs.csproj"
COPY . .
WORKDIR "/src/ConnectUs"
RUN dotnet build "ConnectUs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConnectUs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD /bin/bash ../entrypoint.sh
