build:
	@echo "Building backend application..."
	@sudo dotnet restore "./devRoot.Server/devRoot.Server.csproj"
	@sudo dotnet build "./devRoot.Server/devRoot.Server.csproj" -c Development -o /app/build
	@sudo dotnet publish "./devRoot.Server/devRoot.Server.csproj" -c Development -o /app/publish /p:UseAppHost=false

run:
	@echo "Starting backend in development mode!"
	@sudo dotnet ./devRoot.Server/bin/Development/net8.0/devRoot.Server.dll

migrate:
	@echo "Restoring tool: 'dotnet-ef' ..."
	@sudo dotnet tool restore --tool-manifest ./devRoot.Server/.config/dotnet-tools.json
	@echo "Building migrations..."
	@sudo dotnet ef migrations add UpdateDatabase --project ./devRoot.Server/
	@echo "Updating database with migration..."
	@sudo dotnet ef database update --project ./devRoot.Server/
    
clean:
	@echo "Removing old binaries..."
	@sudo rm -rf ./devRoot.Server/bin/ ./devRoot.Server/app/ ./devRoot.Server/obj/

docker:
	@echo "Building docker image..."
	@sudo docker build -t devroot-server:latest .
	@echo "Running built docker container..."
	@sudo docker run -d -p 8080:8080 -p 8081:8081 --name devroot-server devroot-server:latest
 