# ping79_00

Windows Forms IP Ping 트레이 모니터 (.NET Framework 4.7.2).

- 실행 시 기본 IP `192.168.137.79` 검사를 시작합니다.
- 1초 간격으로 비동기 Ping을 전송합니다. 응답 제한 시간은 900ms입니다.
- 트레이 아이콘은 성공 시 파란색, 실패 시 빨간색, 중지 시 회색입니다.
- 툴팁에는 검사 중인 IP와 상태가 표시됩니다.
- 메인 화면에서 IP 변경, Start, Stop, 종료를 실행합니다.
- 트레이 아이콘을 왼쪽이나 오른쪽으로 클릭하면 같은 메뉴가 열립니다. 메인 화면은 메뉴에서 **IP 변경**을 선택해 열 수 있습니다.
- 창의 X 버튼은 트레이로 숨깁니다. 종료 버튼 또는 Exit 메뉴로 종료합니다.
- IP 변경은 현재 실행 중에만 유지됩니다.

Visual Studio에서 `ping79_00.slnx`를 열어 빌드합니다.

개발 환경에는 .NET Framework 4.7.2 대상 팩과 Visual Studio MSBuild가 필요합니다.
Visual Studio Developer PowerShell에서는 다음 명령으로 빌드할 수 있습니다.

```powershell
msbuild ping79_00.slnx /t:Build /p:Configuration=Debug
```

빌드 결과는 `ping79_00/bin/Debug/ping79_00.exe`입니다.
대상 장치의 ICMP 응답이 차단되어 있으면 Ping 상태는 실패로 표시됩니다.

검증: Debug 빌드, `127.0.0.1` Ping 성공, Stop 시 타이머 중지 및 상태 변경,
트레이 메뉴 구성과 폼 리소스 해제를 확인했습니다.
