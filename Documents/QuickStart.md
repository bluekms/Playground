# Quick Start

## Index
1. [Use Docker Compose](#use-docker-compose)
2. [Use Docker Container](#use-separate-docker-containers)
3. [AuthDb Migration](#authdb-migration)
4. [SignUp And LogIn Test](#signup-and-login-test)

1, 3, 4번 또는 2, 3, 4번을 순서대로 실행

## Use Docker Compose

```
cd .\DockerCompose\
docker compose up -d
```

## Use Separate Docker Containers

### docker network
```
docker network create playground-network --subnet 172.21.0.0/24
```

### Redis
```
docker run --name RedisCache `
  --network playground-network `
  --ip 172.21.0.10 `
  -p 6379:6379 `
  -d redis:7.0.14
```

### PostgreSql
```
docker run --name PlaygroundDb `
  --network playground-network `
  --ip 172.21.0.20 `
  -p 5432:5432 `
  -e POSTGRES_PASSWORD=1234 `
  -d postgres:14.9
```

### AuthServer
```
docker run --name AuthServer `
  --network playground-network `
  --ip 172.21.0.30 `
  -p 8080:80 `
  -p 8443:443 `
  -v ${pwd}/mnt/Logs/AuthServer:/app/Logs `
  -e ASPNETCORE_ENVIRONMENT=Docker `
  -d bluekms/playground-auth-server
```

### Use Grafana

#### Loki
```
docker run --name loki `
  --network playground-network `
  --ip 172.21.0.50 `
  -p 3100:3100 `
  -v ${PWD}/mnt/Config:/mnt/config `
  -d grafana/loki:2.9.1 `
    --config.file=/mnt/config/loki-config.yaml
```

`http://localhost:3100/metrics`


#### Promtail
```
docker run --name promtail_AuthServer `
  --network playground-network `
  -v ${PWD}/mnt/Config:/mnt/config `
  -v ${PWD}/mnt/Logs/AuthServer:/var/log `
  -d grafana/promtail:2.9.1 `
    --config.file=/mnt/config/promtail-config.yaml
```


#### Grafana
```
docker run --name grafana `
  --network playground-network `
  -p 3000:3000 `
  -v ${PWD}/mnt/GrafanaStorage:/var/lib/grafana `
  -e "GF_LOG_LEVEL=debug" `
  -e "GF_INSTALL_PLUGINS=grafana-clock-panel, grafana-simple-json-datasource" `
  -d grafana/grafana-oss
```


## AuthDb Migration
```
cd .\Libraries\AuthDb\
$env:ASPNETCORE_ENVIRONMENT='Development'
dotnet ef database update --context AuthDbContext
```

## SignUp And LogIn Test
1. Foo.http (Foo2)
2. SignUp.http
3. Login.http
4. Foo.http (Foo1)