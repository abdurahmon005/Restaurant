# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

```bash
# Build the solution
dotnet build RestaurantProject.sln

# Run the API (from repository root)
dotnet run --project src/WebAPI/RestaurantApp.API.csproj

# Run with hot reload during development
dotnet watch run --project src/WebAPI/RestaurantApp.API.csproj

# Add a new EF Core migration (from repository root)
dotnet ef migrations add <MigrationName> --project src/WebApp.DataAccess --startup-project src/WebAPI

# Apply migrations
dotnet ef database update --project src/WebApp.DataAccess --startup-project src/WebAPI
```

## Architecture

This is a .NET 8 ASP.NET Core Web API for a restaurant management system using Clean Architecture:

```
src/
├── WebAPI/                    # Presentation layer - Controllers, Program.cs
├── WebApp.Aplication/         # Application layer - Services, DTOs, Validators
├── WebApp.DataAccess/         # Data access layer - EF Core DbContext, Migrations
└── WebApp.Domain/             # Domain layer - Entity classes
```

**Layer Dependencies:** WebAPI → Application → DataAccess → Domain

### Key Components

- **Database:** PostgreSQL via Npgsql.EntityFrameworkCore
- **ORM:** Entity Framework Core 9
- **Authentication:** JWT Bearer tokens (configured in `JwtOption` section of appsettings)
- **Validation:** FluentValidation (validators in `WebApp.Aplication/Validators/`)
- **File Storage:** MinIO object storage
- **Telegram Integration:** Telegram.Bot for bot functionality (`RestaurantTelegramBot` hosted service)
- **API Documentation:** Swagger/OpenAPI at `/swagger` in development

### Domain Entities

Core entities in `WebApp.Domain/Entities/`: User, Role, Permission, Order, OrderDetails, Product, Category, Table, Payment, Transaction, Account

### Service Pattern

Services follow interface/implementation pattern:
- Interfaces: `src/WebApp.Aplication/Services/Interface/I*Service.cs`
- Implementations: `src/WebApp.Aplication/Services/Impl/*Service.cs`

### Configuration

Required configuration sections in `appsettings.json`:
- `ConnectionStrings:DefaultConnection` - PostgreSQL connection string
- `JwtOption` - JWT settings (SecretKey, etc.)
- `EmailConfiguration` - SMTP settings for email
- `MinioSettings` - MinIO object storage configuration
- `TelegramBot:Token` - Telegram bot token
