# IP 기록

IP 입력창은 편집 가능한 콤보박스입니다. 오른쪽 화살표를 누르면 최근 IP를
선택할 수 있습니다. 직접 입력하거나 목록에서 선택한 뒤 `IP 변경` 또는
`Start`를 눌러 적용합니다. Enter도 `IP 변경`을 실행합니다.

유효한 IP를 적용하면 기록의 맨 위로 이동합니다. 중복 없이 최근 10개를
보관하며 Ping 응답 성공 여부와 관계없이 기록합니다. 다시 실행하면 가장 최근
IP로 검사를 시작합니다. 기록이 없으면 기본 IP `192.168.137.79`를 사용합니다.

기존 `%LOCALAPPDATA%\ping79_00\settings.ini`의 `[Ping]` 섹션에
`IP1`부터 `IP10`까지 최신순으로 저장합니다. 로그 폴더의 `[LogAnalysis]`
섹션과 다른 섹션은 유지합니다.

빌드 후 검증:

```powershell
powershell -NoProfile -STA -ExecutionPolicy Bypass -File tests\Test-IpHistory.ps1
```
