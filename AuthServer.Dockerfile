FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Servers/AuthServer/appsettings.Docker.json", "Servers/AuthServer/"]
COPY ["Servers/AuthServer/AuthServer.csproj", "Servers/AuthServer/"]
COPY ["Libraries/AuthDb/AuthDb.csproj", "Libraries/AuthDb/"]
COPY ["Libraries/AuthLibrary/AuthLibrary.csproj", "Libraries/AuthLibrary/"]
COPY ["Libraries/CommonLibrary/CommonLibrary.csproj", "Libraries/CommonLibrary/"]
COPY ["Libraries/Protobuf/Protobuf.csproj", "Libraries/Protobuf/"]
RUN dotnet restore "Servers/AuthServer/AuthServer.csproj"
COPY . .
WORKDIR "/src/Servers/AuthServer"
RUN dotnet build "AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthServer.dll"]
