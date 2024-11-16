build:
	@echo "Building backend application..."
	@sudo dotnet restore "./devRoot.Server/devRoot.Server.csproj"
	@sudo dotnet build "./devRoot.Server/devRoot.Server.csproj" -c Development -o /app/build
	@sudo dotnet publish "./devRoot.Server/devRoot.Server.csproj" -c Development -o /app/publish /p:UseAppHost=false

run:
	@echo "Starting backend in development mode!"
	@sudo dotnet ./devRoot.Server/bin/Development/net8.0/devRoot.Server.dll

clean:
	@echo "Removing old binaries..."
	@sudo rm -rf ./devRoot.Server/bin/