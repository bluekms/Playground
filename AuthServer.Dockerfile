FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Build.targets", "./"]
COPY ["Servers/AuthServer/appsettings.Docker.json", "Servers/AuthServer/"]
COPY ["Servers/AuthServer/AuthServer.csproj", "Servers/AuthServer/"]
COPY ["Libraries/Analyzers/Analyzers.csproj", "Libraries/Analyzers/"]
COPY ["Libraries/AuthDb/AuthDb.csproj", "Libraries/AuthDb/"]
COPY ["Libraries/AuthLibrary/AuthLibrary.csproj", "Libraries/AuthLibrary/"]
COPY ["Libraries/CommonLibrary/CommonLibrary.csproj", "Libraries/CommonLibrary/"]
COPY ["Libraries/Protobuf/Protobuf.csproj", "Libraries/Protobuf/"]

# 새로운 코드 추가: Libraries/Protobuf/obj/ 경로에 .editorconfig 파일 생성 및 내용 작성
RUN mkdir -p Libraries/Protobuf/obj/ && \
    echo "# NoWarn Generated Files" > Libraries/Protobuf/obj/.editorconfig && \
    echo "[*.cs]" >> Libraries/Protobuf/obj/.editorconfig && \
    echo "dotnet_diagnostic.SA1518.severity = none" >> Libraries/Protobuf/obj/.editorconfig \

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
