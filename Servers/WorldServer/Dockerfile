FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Servers/WorldServer/appsettings.Docker.json", "Servers/WorldServer/"]
COPY ["Servers/WorldServer/WorldServer.csproj", "Servers/WorldServer/"]
COPY ["Libraries/AuthDb/AuthDb.csproj", "Libraries/AuthDb/"]
COPY ["Libraries/CommonLibrary/CommonLibrary.csproj", "Libraries/CommonLibrary/"]
COPY ["Libraries/AuthLibrary/AuthLibrary.csproj", "Libraries/AuthLibrary/"]
RUN dotnet restore "Servers/WorldServer/WorldServer.csproj"
COPY . .
WORKDIR "/src/Servers/WorldServer"
RUN dotnet build "WorldServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorldServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorldServer.dll"]
