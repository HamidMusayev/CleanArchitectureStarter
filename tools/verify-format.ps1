# Jump to the folder where this script lives
Set-Location (Split-Path -Parent $MyInvocation.MyCommand.Path)

Write-Host "Verifying code format..."

# Ensure dotnet-format is installed
if (-not (Get-Command dotnet-format -ErrorAction SilentlyContinue)) {
    Write-Host "dotnet-format not found. Installing..."
    dotnet tool install -g dotnet-format
    $env:PATH += ";$($env:USERPROFILE)\.dotnet\tools"
} else {
    Write-Host "dotnet-format is already installed."
}

# Move one level up (parent of tools)
#Set-Location ..

# Run dotnet format in check mode, writing the report back into tools
dotnet format .. --report ./format-report.json

if ($LASTEXITCODE -ne 0) {
    Write-Error "Code formatting issues found."
    exit 1
}