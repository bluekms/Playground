# Src
그라파나 사이트에서 다운로드받은 loki와 promtail의 설정파일 원본

# mnt
볼륨마운트를 위한 디렉토리
gitignore에 등록되어 있음

## Config
Src에서 복사해서 사용
필요한 내용을 첨삭

## GrafanaStorage
Docker Grafana 이미지를 사용하면 사용되는 장소

## Logs
Docker Compose를 이용해 실행되는 Playground 서버들의 로그와 마운트
`Logs/{서버이미지명}` 형식으로 사용


# Grafana 참고자료
https://grafana.com/docs/loki/latest/get-started/

https://grafana.com/docs/loki/latest/setup/install/docker/
https://raw.githubusercontent.com/grafana/loki/v2.9.1/cmd/loki/loki-local-config.yaml

https://grafana.com/docs/loki/latest/send-data/promtail/
https://raw.githubusercontent.com/grafana/loki/v2.9.1/clients/cmd/promtail/promtail-docker-config.yaml
https://grafana.com/docs/loki/latest/send-data/promtail/stages/json/
