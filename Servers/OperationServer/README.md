# OperationServer
* Admin 계정으로 User이외의 AccountRole을 가진 계정을 생성
* 개발에 필요한 각 툴의 요청을 처리하는 서버

## How to use

### 네트워크 준비
```
docker network create playground-network
```

### 이미지 다운로드
```
docker run --name OperationServer --network playground-network -p 5341:80 -p 7341:443 -v playground-volume-operation-server:/app -e ASPNETCORE_ENVIRONMENT=Docker -d bluekms/playground-operation-server:latest
```
|파라메터|값|설명|
|:---|:---|:---|
|name|OperationServer|컨테이너 명|
|network|playground-network|Docker 네트워크 명|
|p|5341:80|http 포트 매핑|
|p|7341:443|http2 포트 매핑|
|v|playground-volume-operation-server:/app|로그 및 StaticData 주입을 위한 볼륨마운트|
|e|ASPNETCORE_ENVIRONMENT=Docker|dotnet 환경변수 명|
|d||
|bluekms/playground-operation-server:latest|docker hub 이미지 명|

### StaticData 주입
`\\wsl$\docker-desktop-data\data\docker\volumes\_data\StaticDataSrc` 폴더 생성 후
`Playground/StaticData/__TestStaticData/Output/StaticData-latest.tar.gz` 복사


## Remove OperationServer
```
docker container rm OperationServer -f
docker image rm bluekms/playground-operation-server -f
docker volume rm playground-volume-operation-server -f
```
