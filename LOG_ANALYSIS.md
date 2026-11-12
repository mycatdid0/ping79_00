# 로그 분석

트레이 메뉴의 `Stop → 구분선 → 로그 분석 → 구분선 → Exit`에서 엽니다.
`LogAnalysisForm.cs`의 디자이너 보기로 창과 메뉴를 편집할 수 있습니다.
파일별 TextBox는 로딩 시 생성됩니다.

- 처음 열 때 자동 로딩합니다. 이미 열린 창은 다시 활성화합니다.
- `폴더 변경...`에서 선택하면 설정을 저장하고 즉시 재로딩합니다.
- `재로딩` 또는 F5로 다시 읽습니다.
- 기본 폴더: `E:\temp\_uart_log`
- 설정 파일: `%LOCALAPPDATA%\ping79_00\settings.ini`

```ini
[LogAnalysis]
Folder=E:\temp\_uart_log
```

하위 폴더는 검색하지 않습니다. 파일 확장자에 관계없이 최종 수정 시각이
가장 최근인 10개를 최신순으로 표시합니다. 같은 시각이면 파일명 순입니다.
각 TextBox는 읽기 전용이며 마우스 선택과 Ctrl+C를 지원합니다.
줄 바꿈 없이 가로 스크롤하고, 전체 목록은 세로 스크롤합니다.

`build time:`, ` git (`, `chip id (serial-number)`, `AI board:`,
`Kernel command line:`이 포함된 줄을 원래 순서대로 추출합니다.
Git 정보 직후의 `* 커밋해시 설명` 줄은 최대 3개 포함합니다.
날짜와 시각 접두사는 제거하고 예제의 부팅 경과 시간 `[0.290]`,
`[    0.000000]`은 유지합니다. UTF-8을 기본으로 읽으며 BOM이 있으면
해당 인코딩을 사용합니다. 기록 중인 파일도 공유 읽기로 접근합니다.

폴더 접근 실패는 메시지로, 개별 파일 읽기 실패는 해당 파일의 TextBox로 표시합니다.

빌드 후 검증:

```powershell
powershell -NoProfile -STA -ExecutionPolicy Bypass -File tests\Test-LogAnalysis.ps1
```
