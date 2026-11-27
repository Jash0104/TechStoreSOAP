$ErrorActionPreference = "Stop"
$vswhere = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
$msbuild = & $vswhere -latest -prerelease -requires Microsoft.Component.MSBuild -find "MSBuild\**\Bin\MSBuild.exe" | Select-Object -First 1
if (-not $msbuild) { throw "No se encontró MSBuild" }
foreach ($p in "TechStore.Servicio", "TechStore.Cliente") {
    & $msbuild (Join-Path $PSScriptRoot "$p\$p.csproj") /p:Configuration=Debug /v:minimal /nologo
    if ($LASTEXITCODE -ne 0) { throw "La compilación de $p falló" }
}
