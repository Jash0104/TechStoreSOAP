# TechStore SOAP

Servicio web SOAP (ASMX) y cliente Web Forms en ASP.NET sobre .NET Framework 4.7.2.

| Proyecto | URL |
|---|---|
| TechStore.Servicio | http://localhost:51080/ProductoService.asmx (WSDL: `?wsdl`) |
| TechStore.Cliente | http://localhost:51081/Default.aspx |

## Ejecutar en Visual Studio
1. Abrir `TechStoreSOAP.slnx`.
2. En el selector de inicio elegir **Servicio y Cliente** (o configurar varios proyectos de inicio).
3. Presionar F5.

## Ejecutar desde PowerShell
    .\compilar.ps1
    .\ejecutar.ps1

Los productos se guardan en memoria: al reiniciar el servicio vuelven los datos iniciales.
