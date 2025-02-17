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
function migrate {
    Write-Output "Restoring tool: 'dotnet-ef' ..."
    dotnet tool restore --tool-manifest .\devRoot.Server\.config\dotnet-tools.json
    Write-Output "Building migration 'UpdateDatabase'..."
    dotnet ef migrations add UpdateDatabase --project .\devRoot.Server\
    Write-Output "Updating database with migration..."
    dotnet ef database update --project .\devRoot.Server\
}

function dockr {
    Write-Output "Building docker container..."
    docker build -t devroot-server:latest .
    Write-Output "Running built docker container..."
    docker run -d -p 8080:8080 -p 8081:8081 --name devroot-server devroot-server:latest
}


if ($args.Count -eq 0) {
    Write-Output "No parameters provided. Usage:"
    Write-Output ".\setup.ps1 [clean] [build] [run] [migrate] [docker]"
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
            
            "migrate" {
                migrate
            }
            
            "docker" {
                dockr
            }

            Default {
                Write-Host "Unkown paramter: $($parameter)"
            }
        }
    }
}