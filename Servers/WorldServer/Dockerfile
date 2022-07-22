FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Servers/WorldServer/WorldServer.csproj", "WorldServer/"]
COPY ["Libraries/CommonLibrary/CommonLibrary.csproj", "CommonLibrary/"]
RUN dotnet restore "Servers/WorldServer/WorldServer.csproj"
COPY . .
WORKDIR "/src/WorldServer"
RUN dotnet build "WorldServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorldServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorldServer.dll"]
