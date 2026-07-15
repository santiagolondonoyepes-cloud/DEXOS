Param(
    [switch]$BuildApi
)

$ErrorActionPreference = 'Stop'

Set-Location "$PSScriptRoot\.."

Write-Host "[DEXOS] Starting SQL Server + Observability stack..."
docker compose -f docker-compose.observability.yml up -d

Write-Host "[DEXOS] Waiting for SQL Server to accept connections..."
Start-Sleep -Seconds 15

Write-Host "[DEXOS] Applying EF Core migrations..."
& "C:\Program Files\dotnet\dotnet.exe" ef database update --project src/DEXOS.Infrastructure/DEXOS.Infrastructure.csproj --startup-project src/DEXOS.API/DEXOS.API.csproj

if ($BuildApi) {
    Write-Host "[DEXOS] Building API..."
    & "C:\Program Files\dotnet\dotnet.exe" build DEXOS.sln
}

Write-Host "[DEXOS] Infrastructure ready."
