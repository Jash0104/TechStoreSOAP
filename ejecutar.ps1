# Abre el servicio (51080) y el cliente (51081) en IIS Express, cada uno en su ventana.
$iis = Join-Path $env:ProgramFiles "IIS Express\iisexpress.exe"
$sitios = @{ "TechStore.Servicio" = 51080; "TechStore.Cliente" = 51081 }
foreach ($s in $sitios.GetEnumerator()) {
    Start-Process $iis -ArgumentList "/path:`"$(Join-Path $PSScriptRoot $s.Key)`"", "/port:$($s.Value)", "/clr:v4.0"
}
Start-Sleep -Seconds 3
Start-Process "http://localhost:51081/Default.aspx"
