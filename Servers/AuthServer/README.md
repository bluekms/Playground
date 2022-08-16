# AuthServer
* 인증 및 인가를 담당
* 유저 및 서버들의 롤 관리

## How to use
```
docker run --name AuthServer --network playground-network -e ASPNETCORE_ENVIRONMENT=Docker -d -p 5241:80 -p 7241:443 bluekms/playground-auth-server:latest
```
* name AuthServer : 컨테이너 명
* network playground-network : Docker 네트워크 명
* e ASPNETCORE_ENVIRONMENT=Docker : dotnet 환경변수 명
* p 5241:80 : http 포트 매핑
* p 7241:443 : http2 포트 매핑
* bluekms/playground-auth-server:latest : docker hub 이미지 명

## Remove AuthServer
```
docker container rm AuthServer -f
docker image rm bluekms/playground-auth-server -f
```