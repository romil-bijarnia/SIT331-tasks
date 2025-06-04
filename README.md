# SIT331 Task 4.1P

This repository contains a sample implementation of the SIT331 Task 4.1P pass submission. It demonstrates a basic persistence layer for a Robot Controller API using PostgreSQL with `Npgsql`.

## Project Structure

- `SIT331_4.1P/` - main project files
  - `4.1P.csproj` - ASP.NET Core project file
  - `RobotCommandsController.cs` / `MapsController.cs` - API controllers
  - `RobotCommandDataAccess.cs` / `MapDataAccess.cs` - persistence layer
  - `4.1P-schema.sql` - database schema dump
  - `Models/` - domain models

## Building

The project targets .NET 8.0, so a compatible SDK is required. After installing the proper .NET SDK run:

```bash
dotnet build SIT331_4.1P/4.1P.csproj
```

## Database

Use PostgreSQL 14 or later and execute `4.1P-schema.sql` to create the required tables and seed data.
