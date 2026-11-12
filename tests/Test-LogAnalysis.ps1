$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
$assembly = [Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '..\ping79_00\bin\Debug\ping79_00.exe'))
$flags = [Reflection.BindingFlags]'Static,NonPublic'
$analysis = $assembly.GetType('ping79_00.LogAnalysis')
$extract = $analysis.GetMethod('Extract', $flags)
$load = $analysis.GetMethod('Load', $flags)
$fixture = Join-Path ([IO.Path]::GetTempPath()) ('ping79-log-test-' + [Guid]::NewGuid())
[IO.Directory]::CreateDirectory($fixture) | Out-Null
function Assert($condition, $message) { if (-not $condition) { throw $message } }
try {
    $sample = @'
[2026-11-12 13:18:26.123] [0.290] [AI-BRINGUP][UBOOT] build time: 26.11.11_15:00:00
13:18:26.124 [0.293] [AI-BRINGUP][UBOOT] git (/home/build):
* b89293e first
* 77348c9 second
* 71881f8 third
* 1234567 fourth
unrelated line
[0.770] [AI-BRINGUP][UBOOT] chip id (serial-number): 0821A3BA4A9
[    0.000000] [AI-BRINGUP][KERNEL] build time: 26.11.11_15:56:20
[    0.000000] [AI-BRINGUP][KERNEL] git (/home/build):
[    0.000000] * b1a6154 kernel first
[    0.000000] * b89293e kernel second
[    0.000000] * 77348c9 kernel third
[    0.000000] [AI-BRINGUP][KERNEL] chip id (serial-number): 0821A3BA4A9
[    0.000000] Kernel command line: root=/dev/sdc4
[    0.002984] AI board: DroneAI GPAK=0
[    0.003071] AI board: packed 3 external IRQ sources
[    0.003441] AI board: 44Disp=0 UART3=enabled
'@
    $path = Join-Path $fixture 'sample.txt'
    [IO.File]::WriteAllText($path, $sample)
    $writer = [IO.FileStream]::new($path, [IO.FileMode]::Open, [IO.FileAccess]::ReadWrite, [IO.FileShare]::ReadWrite)
    try { $result = $extract.Invoke($null, @([string]$path)) } finally { $writer.Dispose() }
    Assert ($result.StartsWith('FILE: ' + $path)) 'Absolute path header missing'
    Assert (-not $result.Contains('2026-11-12')) 'Wall clock date retained'
    Assert (-not $result.Contains('13:18:26')) 'Wall clock time retained'
    Assert ($result.Contains('[0.290]')) 'UBOOT boot timestamp removed'
    Assert ($result.Contains('[    0.000000] * b1a6154')) 'Kernel commit or timestamp missing'
    Assert (-not $result.Contains('fourth')) 'More than three commits extracted'
    Assert (-not $result.Contains('unrelated')) 'Unrelated line extracted'
    Assert (($result -split '\r?\n').Count -eq 17) 'Expected header and all 16 selected lines'

    [IO.File]::Delete($path)
    Assert ($load.Invoke($null, @([string]$fixture)).Count -eq 0) 'Empty folder result incorrect'
    for ($i = 0; $i -lt 12; $i++) {
        $path = Join-Path $fixture ('log-{0:D2}.txt' -f $i)
        [IO.File]::WriteAllText($path, '[0.1] build time: test')
        [IO.File]::SetLastWriteTimeUtc($path, [DateTime]::UtcNow.AddMinutes(-$i))
    }
    $results = $load.Invoke($null, @([string]$fixture))
    Assert ($results.Count -eq 10) 'Latest-ten limit incorrect'
    Assert ($results[0].Contains('log-00.txt')) 'Newest file not first'
    Assert ($results[9].Contains('log-09.txt')) 'Incorrect file ordering'

    $form = [Activator]::CreateInstance($assembly.GetType('ping79_00.LogAnalysisForm'))
    try {
        $instanceFlags = [Reflection.BindingFlags]'Instance,NonPublic'
        $panel = $form.GetType().GetField('resultsPanel', $instanceFlags).GetValue($form)
        $reload = $form.GetType().GetMethod('ReloadAsync', $instanceFlags)
        $form.GetType().GetField('folder', $instanceFlags).SetValue($form, [string]$fixture)
        # Create a window handle so async continuations return to the UI thread.
        $null = $form.Handle
        $task = $reload.Invoke($form, @())
        $deadline = [DateTime]::UtcNow.AddSeconds(15)
        while (-not $task.IsCompleted) {
            [Windows.Forms.Application]::DoEvents()
            if ([DateTime]::UtcNow -gt $deadline) { throw 'Reload timed out' }
            Start-Sleep -Milliseconds 10
        }
        $null = $task.GetAwaiter().GetResult()
        Assert ($panel.Controls.Count -eq 10) 'Expected ten TextBoxes'
        Assert ($panel.Controls[0].Text.Contains('log-00.txt')) 'Newest TextBox not first'
        foreach ($box in $panel.Controls) {
            Assert ($box -is [Windows.Forms.TextBox] -and $box.ReadOnly -and -not $box.WordWrap) 'Incorrect TextBox configuration'
            Assert ($box.Height -ge $box.Lines.Count * $box.Font.Height) 'TextBox too short'
        }
        $form.ClientSize = [Drawing.Size]::new(700, 400)
        Assert ($panel.Controls[0].Width -le $panel.ClientSize.Width) 'TextBox exceeds panel width'
        $task = $reload.Invoke($form, @())
        while (-not $task.IsCompleted) { [Windows.Forms.Application]::DoEvents(); Start-Sleep -Milliseconds 10 }
        $null = $task.GetAwaiter().GetResult()
        Assert ($panel.Controls.Count -eq 10) 'Reload duplicated results'
    } finally { $form.Dispose() }

    $main = [Activator]::CreateInstance($assembly.GetType('ping79_00.Form1'))
    try {
        $menu = $main.GetType().GetField('trayMenu', $instanceFlags).GetValue($main)
        Assert ($menu.Items[2].Text -eq 'Stop') 'Stop menu misplaced'
        Assert ($menu.Items[3] -is [Windows.Forms.ToolStripSeparator]) 'Missing first separator'
        Assert ($menu.Items[4].Name -eq 'logAnalysisMenu') 'Log menu misplaced'
        Assert ($menu.Items[5] -is [Windows.Forms.ToolStripSeparator]) 'Missing second separator'
        Assert ($menu.Items[6].Text -eq 'Exit') 'Exit menu misplaced'
    } finally { $main.Dispose() }
    'PASS: extraction, shared file, latest ten, empty folder, form loading/reloading, TextBox layout, tray menu'
} finally {
    # Only remove this test's unique temporary fixture directory.
    [IO.Directory]::Delete($fixture, $true)
}
