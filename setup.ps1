function build {
    Write-Output "Building backend application..."
    dotnet restore .\devRoot.Server\devRoot.Server.csproj
	dotnet build .\devRoot.Server\devRoot.Server.csproj -c Development -o .\devRoot.Server\app\build
	dotnet publish .\devRoot.Server\devRoot.Server.csproj -c Development -o .\devRoot.Server\app\publish /p:UseAppHost=false
}
function run {
    Write-Output "Starting backend in development mode!"
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    dotnet .\devRoot.Server\bin\Development\net8.0\devRoot.Server.dll
}
function cleanup {
    Write-Output "Removing old binaries..."

    $folders = @(
        ".\devRoot.Server\app\",
        ".\devRoot.Server\bin\",
        ".\devRoot.Server\obj\"
    )
    
    foreach ($folder in $folders) {
        Remove-Item -LiteralPath $folder -Force -Recurse -ErrorAction SilentlyContinue
    }
}

if ($args.Count -eq 0) {
    Write-Output "No parameters provided. Usage:"
    Write-Output ".\setup.ps1 [clean] [build] [run]"
} else {
    foreach ($parameter in $args) {
        switch ($parameter) {
            "build" { 
                build
            }

            "run" {
                run
            }

            "clean" {
                cleanup
            }

            Default {
                Write-Host "Unkown paramter: $($parameter)"
            }
        }
    }
}