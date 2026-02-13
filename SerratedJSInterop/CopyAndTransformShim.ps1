# Copies SerratedJSInteropShim.js from this project's wwwroot and transforms for AMD/RequireJS (Uno WasmScripts).
# Usage: .\CopyAndTransformShim.ps1 -SourcePath <path> -DestPath <path>
param(
    [Parameter(Mandatory=$true)][string]$SourcePath,
    [Parameter(Mandatory=$true)][string]$DestPath
)
$content = [System.IO.File]::ReadAllText($SourcePath)
$content = "define(() => {`n`n" + $content
$content = $content -replace 'export \{ SerratedJSInteropShim \};', '});'
$dir = [System.IO.Path]::GetDirectoryName($DestPath)
if (-not [System.IO.Directory]::Exists($dir)) {
    [System.IO.Directory]::CreateDirectory($dir) | Out-Null
}
[System.IO.File]::WriteAllText($DestPath, $content)
