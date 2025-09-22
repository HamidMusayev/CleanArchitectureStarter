dotnet format --verify-no-changes
if ($LASTEXITCODE -ne 0) { Write-Error "Code formatting issues found."; exit 1 }