# Quick Start

## Setup Docker Container

### docker network
```
docker network create playground-network --subnet 172.18.0.0/16
```

### PostgreSql
```
docker run --name PlaygroundDb --network playground-network --ip 172.18.0.2 -e POSTGRES_PASSWORD=1234 -d -p 3307:5432 postgres:14.9
```
* 고정 IP를 사용하는 이유는 appsettings.json 의 ConnectionStrings를 위함


### Redis
```
docker run --name RedisCache --network playground-network --ip 172.18.0.3 -d -p 6380:6379 redis:7.0.14
```
* 고정 IP를 사용하는 이유는 appsettings.json 의 ConnectionStrings를 위함


### AuthServer
```
docker run --name AuthServer --network playground-network --ip 172.18.0.4 -e ASPNETCORE_ENVIRONMENT=Docker -d -p 5241:80 -p 7241:443 bluekms/playground-auth-server
```
* 고정 IP를 사용하는 이유는 WorldServer\appsettings.Docker.json 의 ServerRegistry를 위함

### WorldServer
```
docker run --name WorldServer --network playground-network -e ASPNETCORE_ENVIRONMENT=Docker -d -p 5641:80 -p 7641:443 bluekms/playground-world-server
```

## Setup Database

### AuthDb 마이그레이션
```
$env:ASPNETCORE_ENVIRONMENT='{환경변수명}'
cd .\Libraries\AuthDb
dotnet ef database update --context AuthDbContext
```