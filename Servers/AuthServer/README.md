# AuthServer
* 계정 및 서버들의 롤 관리
* 세션 부여
* World 서버들의 리스트를 관리
* Client 로그인 시 WorldServer들의 리스트를 내려줌

## How to use
```
docker network create playground-network
```
먼저 네트워크가 필요

```
docker run --name AuthServer --network playground-network --ip 172.18.0.4 -p 5241:80 -p 7241:443 -v playground-volume-auth-server:/app -e ASPNETCORE_ENVIRONMENT=Docker -d bluekms/playground-auth-server:latest
```
|파라메터|값|설명|
|:---|:---|:---|
|name|AuthServer|컨테이너 명|
|network|playground-network|Docker 네트워크 명|
|ip|172.18.0.4|WorldServer 등록을 위한 고정 ip|
|p|5241:80|http 포트 매핑|
|p|7241:443|http2 포트 매핑|
|v|playground-volume-auth-server:/app|windows 개발 환경에서 로그를 편히 볼 수 있도록 볼륨마운트|
|e|ASPNETCORE_ENVIRONMENT=Docker|dotnet 환경변수 명|
|d||
|bluekms/playground-auth-server:latest|docker hub 이미지 명|

## Remove AuthServer
```
docker container rm AuthServer -f
docker image rm bluekms/playground-auth-server -f
docker volume rm playground-volume-auth-server -f
```
