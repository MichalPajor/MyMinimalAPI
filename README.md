# MyMinimalAPI
.NET 6 MinimalAPI Example. Introduced as part of dotnet 6. Archiected to create http APIs with minimal dependencies. Ideal for microservices that want to include only minimal files, features and dependencies in asp.net core.
![alt text](https://i.imgur.com/Sdvn4tO.png)

# Technologies&Features
- .NET 6 Minimal API
- VS Code
- Entity Framewor Core
- MSSQL Server
- Docker
- Swagger
- Code First
- Secrets
- Data Transfer Objects
- Repositor Pattern
- Auto Mapper
- Middleware

# Step by step
## 1. Create project
- Create project with command:
```bash
dotnet new webapi -minimal -n MyMinimalAPI
```
- Open project in VS Code:
```bash
code -r MyMinimalAPI
```
- Try to first run:
```bash
dotnet run
```
## 2. Add Nuget packages
- Entity Framework Core
```bash
dotnet add package Microsoft.EntityFrameworkCore
```
- Entity Framework Core Design
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```
- Entity Framweork SqlServer
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```
- Auto Mapper & Dependency Injection
```bash
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```
## 3. Init secrets to store sensitive informations
```bash
dotnet user-secrets init
```

## 4. Setup SqlServer in Docker
- add docker-compose.yaml file
- start docker
```bash
docker-compose up -d
```
We can connect to server using tool MSSMS.
## 5. Code First - create models
## 6. Create Data Transfer Objects
## 7. Create DBContext file
## 8. Use Secrets to generate UserId and Password for ConnectionString
```bash
dotnet user-secrets set „UserId” „sa”
```
```bash
dotnet user-secrets set „Password” „mySecretPassword!”
```
## 9. Register DBContext in Program.cs
- create ConnectionString with SqlConnBuilder
- add DBContext to services and set ConnectionString
## 10. Create migrations and database
```bash
dotnet ef migrations add initialmigration 
```
```bash
dotnet ef database update 
```
## 11. Create repository (use DbContext by Repository Pattern)
- create interface
- create repo that implements interface
- register interface in Program.cs
- create Profile to map models to DTOs
- register mapper in Program.cs
## 12. Create Endpoints in Program.cs using mapper and model binding
## 13. Add middleware
- use secret to store X-Api-Key value
```bash
dotnet user-secrets set „X-Api-Key” „mySecretXApiKey!”
```
- create Middleware.cs file
- add Middleware to Program.cs
- configure swagger to use middleware
