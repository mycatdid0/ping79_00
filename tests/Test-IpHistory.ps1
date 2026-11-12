$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
$assembly = [Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '..\ping79_00\bin\Debug\ping79_00.exe'))
$flags = [Reflection.BindingFlags]'Static,NonPublic'
$historyType = $assembly.GetType('ping79_00.IpHistorySettings')
$iniType = $assembly.GetType('ping79_00.IniSettings')
$normalize = $historyType.GetMethod('Normalize', $flags)
$read = $historyType.GetMethod('Read', $flags)
$save = $historyType.GetMethod('Save', $flags)
$writeSection = $iniType.GetMethod('WriteSection', $flags)
$readValue = $iniType.GetMethod('Read', $flags)
$path = [IO.Path]::Combine([IO.Path]::GetTempPath(), ('ping79-history-' + [Guid]::NewGuid() + '.ini'))
function Assert($condition, $message) { if (-not $condition) { throw $message } }
try {
    [string[]]$values = @('invalid', '', ' 127.0.0.1 ', '127.0.0.1', '::1', '0:0:0:0:0:0:0:1')
    $clean = $normalize.Invoke($null, [object[]](,$values))
    Assert ($clean.Count -eq 2 -and $clean[0] -eq '127.0.0.1' -and $clean[1] -eq '::1') 'Validation/normalization/deduplication failed'
    [IO.File]::WriteAllText($path, "[LogAnalysis]`r`nFolder=E:\temp\_uart_log`r`n[Other]`r`nKeep=yes`r`n")
    [string[]]$values = 1..12 | ForEach-Object { '192.168.0.' + $_ }
    $save.Invoke($null, [object[]]@($path, $values))
    $history = $read.Invoke($null, @($path))
    Assert ($history.Count -eq 10 -and $history[9] -eq '192.168.0.10') 'Ten-item limit or roundtrip failed'
    Assert ($readValue.Invoke($null, @($path, 'LogAnalysis', 'Folder')) -eq 'E:\temp\_uart_log') 'IP save erased folder'
    Assert ($readValue.Invoke($null, @($path, 'Other', 'Keep')) -eq 'yes') 'IP save erased unrelated section'
    [string[]]$values = @('192.168.0.5') + @($history)
    $save.Invoke($null, [object[]]@($path, $values))
    $history = $read.Invoke($null, @($path))
    Assert ($history.Count -eq 10 -and $history[0] -eq '192.168.0.5' -and $history[1] -eq '192.168.0.1') 'Recent IP not moved to front'
    $writeSection.Invoke($null, [object[]]@($path, 'LogAnalysis', [string[]]@('Folder=E:\changed')))
    $history = $read.Invoke($null, @($path))
    Assert ($history.Count -eq 10 -and $history[0] -eq '192.168.0.5') 'Folder save erased history'
    $save.Invoke($null, [object[]]@($path, [string[]]@('127.0.0.1')))
    Assert ($read.Invoke($null, @($path)).Count -eq 1) 'Stale INI history keys retained'
    $form = [Activator]::CreateInstance($assembly.GetType('ping79_00.Form1'))
    try {
        $combo = $form.Controls['ipInput']
        Assert ($combo -is [Windows.Forms.ComboBox]) 'IP input is not a ComboBox'
        Assert ($combo.DropDownStyle -eq [Windows.Forms.ComboBoxStyle]::DropDown) 'ComboBox is not editable'
        Assert ($combo.MaxDropDownItems -eq 10) 'Dropdown item limit incorrect'
        $combo.Text = '10.1.2.3'
        Assert ($combo.Text -eq '10.1.2.3') 'Manual IP editing failed'
    } finally { $form.Dispose() }
    'PASS: history validation, deduplication, recency, ten-item limit, INI roundtrip/section preservation, editable ComboBox'
} finally {
    if ([IO.File]::Exists($path)) { [IO.File]::Delete($path) }
}
