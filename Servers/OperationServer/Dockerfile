FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Servers/OperationServer/OperationServer.csproj", "Servers/OperationServer/"]
RUN dotnet restore "Servers/OperationServer/OperationServer.csproj"
COPY . .
WORKDIR "/src/Servers/OperationServer"
RUN dotnet build "OperationServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OperationServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OperationServer.dll"]
