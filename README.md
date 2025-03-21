# game-of-life-api
Description: Conway's Game of Life API is microservice which exposes Rest APIs using .Net Core 8 and Dockerized SQL Server 

Software Needed to run this project 
1. Docker Desktop
2. SQL Server Image for Docker
3. Visual Studio
4. Postman or Swagger UI
5. Docker Compose steps can be found in Infrastructure folder
6. dotnet run --project ConwayGameOfLife.API in git bash

Viewing Arch Diagrams 
- Integrate PlantUML plugin for VS Code and open the *.puml file.
- Another option is to use https://plantuml.com/

## Issue Tracker & Resolutions

- Docker SQL Server container failed => fixed by enabling file sharing and adjusting volume mounts  
- EF Core connection string not initialized => resolved by adding to `appsettings.json` and configuring `DbContext` in `Program.cs`  
- Route constraint `apiVersion` not found => solved by registering API versioning and explorer in `Program.cs`
- Exposed exception messages => fixed by returning generic 500 and logging details to console 
- Long uri path for getting next state => Still working on implementation

