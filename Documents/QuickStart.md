# Quick Start

## Index
1. [Use Docker Compose](#use-docker-compose)
2. [Use Docker Container](#use-separate-docker-containers)
3. [AuthDb Migration](#authdb-migration)
4. [SignUp And LogIn Test](#signup-and-login-test)

1, 3, 4번 혹은 2, 3, 4번을 순서대로 실행 

## Use Docker Compose

### docker compose
```
docker compose up -d
```

## Use Separate Docker Containers

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

## AuthDb Migration
```
$env:ASPNETCORE_ENVIRONMENT='{환경변수명}'
cd .\Libraries\AuthDb
dotnet ef database update --context AuthDbContext
```

## SignUp And LogIn Test
1. Foo.http (Foo2)
2. SignUp.http
3. Login.http
4. Foo.http (Foo1)