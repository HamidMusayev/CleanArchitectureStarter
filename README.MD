# Clean Architecture Starter

A production-ready .NET 9 Web API template built on **Clean Architecture**, **Domain-Driven Design**, and **CQRS**. Clone it, rename the namespaces, and start shipping.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Project Structure](#project-structure)
- [Architecture Overview](#architecture-overview)
- [Configuration](#configuration)
- [Running the API](#running-the-api)
- [Running Tests](#running-tests)
- [API Reference](#api-reference)
- [Adding a New Feature](#adding-a-new-feature)
- [Known Issues](#known-issues)

---

## Prerequisites

| Tool | Version |
|------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 9.0+ |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | any recent |
| [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) | `dotnet tool install -g dotnet-ef` |

---

## Quick Start

```bash
# 1. Clone
git clone <repo-url>
cd CleanArchitectureStarter

# 2. Start PostgreSQL + pgAdmin
docker-compose up -d postgres pgadmin

# 3. Run the API (migrations apply automatically on startup)
dotnet run --project src/Api
```

Swagger UI is available at **http://localhost:5000/swagger** (Development only).

Health endpoints:
- `GET /health/live` — liveness probe (always 200 if the process is up)
- `GET /health/ready` — readiness probe (checks database connectivity)

---

## Project Structure

```
CleanArchitectureStarter/
├── src/
│   ├── Api/                        # Presentation layer (Web API)
│   │   ├── Controllers/v1/         # Versioned controllers
│   │   ├── Endpoints/              # Minimal API endpoints (health)
│   │   ├── Extensions/             # IServiceCollection extensions
│   │   ├── Filters/                # Global exception filter
│   │   ├── Middlewares/            # Exception handling, security headers
│   │   ├── Mapping/                # AutoMapper profiles (API layer)
│   │   ├── Config/                 # Versioning setup
│   │   └── Program.cs
│   │
│   ├── Application/                # Use-case layer — no framework deps
│   │   ├── Abstractions/           # ICurrentUserService, IDomainEventDispatcher, ICacheService
│   │   ├── Common/
│   │   │   ├── Behaviors/          # MediatR pipeline (validation)
│   │   │   ├── Results/            # Result<T> pattern
│   │   │   └── Interfaces/         # IDateTimeProvider
│   │   └── Features/
│   │       └── Users/
│   │           ├── Commands/CreateUser/
│   │           ├── Events/         # Domain event handlers
│   │           └── Queries/GetUserById/
│   │
│   ├── Domain/                     # Pure domain model — no external deps
│   │   ├── Common/                 # Entity, AggregateRoot, ValueObject, DomainEvent, IAuditable
│   │   ├── Users/
│   │   │   ├── User.cs             # Aggregate root
│   │   │   ├── Events/             # UserCreatedDomainEvent
│   │   │   └── ValueObjects/       # Email
│   │   ├── Repositories/           # IUserRepository (abstraction)
│   │   └── Errors/                 # Domain error codes
│   │
│   ├── Infrastructure/             # EF Core, external services, implementations
│   │   ├── Interceptors/           # AuditableEntitySaveChangesInterceptor
│   │   ├── Migrations/             # EF Core migrations
│   │   ├── Persistence/            # MyAppDbContext + entity configurations
│   │   ├── Repositories/           # UserRepository
│   │   └── Services/               # EmailSender, CurrentUserService, CacheService, etc.
│   │
│   ├── Contracts/                  # Versioned request/response DTOs
│   │   ├── Common/                 # PageRequest, PageResult
│   │   └── v1/Users/
│   │
│
├── tests/
│   ├── UnitTests/                  # Domain + Application layer (xUnit, NSubstitute)
│   ├── IntegrationTests/           # HTTP endpoint tests (WebApplicationFactory)
│   └── FunctionalTests/            # End-to-end smoke tests
│
├── docs/
│   └── architecture-decision-records.md
├── .editorconfig
├── .github/workflows/ci.yml        # CI: build → test → docker
├── Directory.Build.props           # Global: nullable, TreatWarningsAsErrors
├── docker-compose.yaml
└── Dockerfile
```

---

## Architecture Overview

```
┌─────────────────────────────────────┐
│              Api (HTTP)             │  Controllers, Middleware, Filters
└────────────────┬────────────────────┘
                 │ sends commands/queries via MediatR
┌────────────────▼────────────────────┐
│           Application               │  Use cases, Behaviors, Result<T>
└────────────────┬────────────────────┘
                 │ uses abstractions from
┌────────────────▼────────────────────┐
│              Domain                 │  Entities, Value Objects, Domain Events
└────────────────┬────────────────────┘
                 │ implemented by
┌────────────────▼────────────────────┐
│          Infrastructure             │  EF Core, PostgreSQL, Email, Cache
└─────────────────────────────────────┘
```

**Dependency rule:** outer layers depend on inner layers; inner layers never reference outer ones. Infrastructure implements the interfaces declared in Domain and Application.

### Key patterns

| Pattern | Where |
|---------|-------|
| CQRS | `Application/Features/**/Commands` & `Queries` |
| MediatR pipeline | `ValidationBehavior` runs FluentValidation before every handler |
| Domain Events | Raised in aggregates → dispatched after `SaveChangesAsync` via `IDomainEventDispatcher` → handled by `INotificationHandler<T>` |
| Result<T> | Commands return `Result<T>` instead of throwing; controllers map to HTTP status codes |
| Specification | `Specification<T>` abstract class for composable query predicates |
| Audit | `IAuditable` entities get `CreatedAt/By` and `UpdatedAt/By` stamped automatically |

---

## Configuration

All settings live in `src/Api/appsettings.json`. Override per-environment in `appsettings.{Environment}.json` or via environment variables.

### Database

```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=test_db;Username=dis;Password=dis"
}
```

Docker Compose sets `ConnectionStrings__Default` automatically when running the full stack.

### JWT Authentication

```json
"Jwt": {
  "Issuer": "CleanArchitectureApi",
  "Audience": "CleanArchitectureApiUsers",
  "SecretKey": "your-super-secret-key-change-this-in-production-minimum-256-bits!!",
  "ExpirationMinutes": 60
}
```

> **Before deploying:** replace `SecretKey` with a cryptographically random value and store it in a secrets manager (Azure Key Vault, AWS Secrets Manager, etc.).

### CORS

```json
"Cors": {
  "AllowedOrigins": ["http://localhost:3000", "http://localhost:4200"]
}
```

### Logging (Serilog)

Configured via `appsettings.json` under the `Serilog` key. Development overrides log level to `Debug`. Logs are written to the console and to `logs/log-{date}.txt`.

---

## Running the API

### Option A — Local (without Docker)

```bash
# Start PostgreSQL separately (or use an existing instance)
docker-compose up -d postgres

# Run the API
dotnet run --project src/Api
```

### Option B — Full Docker Compose stack

```bash
docker-compose up --build
```

Services:
| Service | URL |
|---------|-----|
| API | http://localhost:5000 |
| Swagger | http://localhost:5000/swagger *(Development only)* |
| pgAdmin | http://localhost:8080 (admin@admin.com / admin) |
| PostgreSQL | localhost:5432 (dis / dis) |

### Applying migrations manually

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/Api
```

Migrations also run automatically on API startup via `db.Database.Migrate()` in `Program.cs`.

---

## Running Tests

```bash
# Unit tests (fast, no database needed)
dotnet test tests/UnitTests

# Integration tests (requires a running PostgreSQL instance)
dotnet test tests/IntegrationTests

# Functional / smoke tests
dotnet test tests/FunctionalTests

# All tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### CI

GitHub Actions runs on every push and pull request to `master`:
1. **Build** — `dotnet build` with `TreatWarningsAsErrors`
2. **Test** — all projects, spins up a PostgreSQL service container
3. **Docker** — verifies the image builds successfully

---

## API Reference

All endpoints are versioned under `/api/v1/`.

### Users

| Method | Path | Description |
|--------|------|-------------|
| `POST` | `/api/v1/users` | Create a new user |
| `GET` | `/api/v1/users/{id}` | Get user by ID |

**Create user — request body:**
```json
{
  "email": "ada@example.com",
  "givenName": "Ada",
  "familyName": "Lovelace"
}
```

**Create user — 201 response:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "ada@example.com",
  "fullName": "Ada Lovelace",
  "createdAt": "2026-05-27T10:00:00Z"
}
```

### Health

| Method | Path | Description |
|--------|------|-------------|
| `GET` | `/health/live` | Liveness — is the process up? |
| `GET` | `/health/ready` | Readiness — is the database reachable? |

---

## Adding a New Feature

Use the `Users` feature as a template. For a new `Products` feature:

**1. Domain** — add entity, value objects, repository interface:
```
src/Domain/Products/Product.cs
src/Domain/Repositories/IProductRepository.cs
```

**2. Application** — add command/query, handler, validator:
```
src/Application/Features/Products/Commands/CreateProduct/
    CreateProductCommand.cs
    CreateProductCommandHandler.cs
    CreateProductCommandValidator.cs
src/Application/Features/Products/Queries/GetProductById/
    GetProductByIdQuery.cs
    GetProductByIdQueryHandler.cs
```

**3. Infrastructure** — add repository implementation and EF configuration:
```
src/Infrastructure/Repositories/ProductRepository.cs
src/Infrastructure/Persistence/Configurations/ProductConfiguration.cs
```
Register in `Infrastructure/DependencyInjection.cs`.

**4. Contracts** — add request/response DTOs:
```
src/Contracts/v1/Products/CreateProductRequest.cs
src/Contracts/v1/Products/ProductResponse.cs
```

**5. Api** — add controller and AutoMapper profile:
```
src/Api/Controllers/v1/ProductsController.cs
src/Api/Mapping/ProductMappingProfile.cs
```

**6. Migrate:**
```bash
dotnet ef migrations add AddProducts --project src/Infrastructure --startup-project src/Api
```

---

## Known Issues

| Issue | Status |
|-------|--------|
| AutoMapper 15.0.1 — high-severity vulnerability [GHSA-rvv3-g6hj-g44x](https://github.com/advisories/GHSA-rvv3-g6hj-g44x) | Suppressed via `NuGetAuditSuppress`; upgrade when a patched version is released |
| MediatR 13+ — commercial license required for production ([Lucky Penny](https://luckypennysoftware.com)) | Allowed for development; evaluate licensing before shipping |
| Email service — stub implementation (logs to console) | Replace `Infrastructure/Services/EmailSender.cs` with SMTP/SendGrid |
| Cache — in-memory only | Replace `MemoryCacheService` with a Redis-backed `IDistributedCache` for multi-instance deployments |
