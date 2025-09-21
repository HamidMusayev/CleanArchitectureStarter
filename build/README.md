Solid, **DDD + Clean Architecture** solution layout for an ASP.NET Core Web API.
Every folder below contains **at least one concrete item** so you can copy-paste this as a starter template.

```
MyApp/
├─ MyApp.sln
├─ build/
│  └─ README.md
├─ docs/
│  └─ architecture-decision-records.md
├─ src/
│  ├─ MyApp.Api/                         # Presentation (Web)
│  │  ├─ Controllers/
│  │  │  └─ V1/
│  │  │     └─ UsersController.cs
│  │  ├─ Endpoints/                      # If using Minimal APIs
│  │  │  └─ HealthEndpoints.cs
│  │  ├─ Filters/
│  │  │  └─ GlobalExceptionFilter.cs
│  │  ├─ Middleware/
│  │  │  └─ ExceptionHandlingMiddleware.cs
│  │  ├─ Mapping/
│  │  │  └─ UserMappingProfile.cs
│  │  ├─ Config/
│  │  │  └─ ApiVersioningSetup.cs
│  │  ├─ Extensions/
│  │  │  └─ ServiceCollectionExtensions.cs
│  │  ├─ Program.cs
│  │  ├─ appsettings.json
│  │  └─ appsettings.Development.json
│  │
│  ├─ MyApp.Application/                 # Use cases (CQRS), no framework deps
│  │  ├─ Abstractions/
│  │  │  ├─ Messaging/
│  │  │  │  └─ IEmailSender.cs
│  │  │  └─ Persistence/
│  │  │     └─ IUnitOfWork.cs
│  │  ├─ Common/
│  │  │  ├─ Behaviors/
│  │  │  │  └─ ValidationBehavior.cs
│  │  │  ├─ Exceptions/
│  │  │  │  └─ NotFoundException.cs
│  │  │  ├─ Interfaces/
│  │  │  │  └─ IDateTimeProvider.cs
│  │  │  ├─ Mappings/
│  │  │  │  └─ ApplicationMappingProfile.cs
│  │  │  └─ Results/
│  │  │     └─ Result.cs
│  │  ├─ Features/
│  │  │  └─ Users/
│  │  │     ├─ Commands/
│  │  │     │  └─ CreateUser/
│  │  │     │     ├─ CreateUserCommand.cs
│  │  │     │     ├─ CreateUserCommandHandler.cs
│  │  │     │     └─ CreateUserCommandValidator.cs
│  │  │     └─ Queries/
│  │  │        └─ GetUserById/
│  │  │           ├─ GetUserByIdQuery.cs
│  │  │           └─ GetUserByIdQueryHandler.cs
│  │  └─ DependencyInjection.cs
│  │
│  ├─ MyApp.Domain/                      # Pure domain model
│  │  ├─ Common/
│  │  │  ├─ Entity.cs
│  │  │  ├─ AggregateRoot.cs
│  │  │  ├─ ValueObject.cs
│  │  │  └─ DomainEvent.cs
│  │  ├─ Users/
│  │  │  ├─ User.cs
│  │  │  ├─ Events/
│  │  │  │  └─ UserCreatedDomainEvent.cs
│  │  │  └─ ValueObjects/
│  │  │     └─ Email.cs
│  │  ├─ Repositories/
│  │  │  └─ IUserRepository.cs
│  │  └─ Errors/
│  │     └─ DomainErrors.cs
│  │
│  ├─ MyApp.Infrastructure/              # EF Core, external services, implementations
│  │  ├─ Persistence/
│  │  │  ├─ MyAppDbContext.cs
│  │  │  ├─ Configurations/
│  │  │  │  └─ UserConfiguration.cs
│  │  │  └─ Migrations/
│  │  │     └─ 202409201200_Initial.cs
│  │  ├─ Repositories/
│  │  │  └─ UserRepository.cs
│  │  ├─ Services/
│  │  │  └─ EmailSender.cs
│  │  ├─ Interceptors/
│  │  │  └─ AuditableEntitySaveChangesInterceptor.cs
│  │  └─ DependencyInjection.cs
│  │
│  ├─ MyApp.Contracts/                   # DTOs (request/response), versioned
│  │  ├─ Common/
│  │  │  ├─ PageRequest.cs
│  │  │  └─ PageResult.cs
│  │  └─ V1/
│  │     └─ Users/
│  │        ├─ CreateUserRequest.cs
│  │        └─ UserResponse.cs
│  │
│  └─ MyApp.SharedKernel/                # Optional: cross-domain primitives
│     ├─ GuardClauses/
│     │  └─ Guard.cs
│     └─ Specs/
│        └─ Specification.cs
│
├─ tests/
│  ├─ MyApp.UnitTests/
│  │  ├─ Domain/
│  │  │  └─ Users/
│  │  │     └─ UserTests.cs
│  │  └─ Application/
│  │     └─ Users/
│  │        └─ CreateUserCommandTests.cs
│  ├─ MyApp.IntegrationTests/
│  │  └─ Api/
│  │     └─ Users/
│  │        └─ CreateUserEndpointTests.cs
│  └─ MyApp.FunctionalTests/
│     └─ BasicSmokeTests.cs
│
├─ tools/
│  └─ verify-format.ps1
├─ Directory.Build.props
└─ Directory.Build.targets
```

## Quick notes (why this is “clean”)

* **Api** only handles HTTP concerns, mapping, validation, and wires pipelines.
* **Application** is your use-case layer (CQRS, behaviors, no EF Core or Web).
* **Domain** is pure C# (entities, VOs, domain events, repos abstractions).
* **Infrastructure** implements abstractions (EF Core `DbContext`, repositories, email, etc.) and is the only layer that talks to the outside world.
* **Contracts** holds versioned DTOs so your public API can evolve without leaking domain/persistence.
* **SharedKernel** (optional) stores cross-cutting primitives used by multiple bounded contexts.
* **tests** split by intent: unit (domain/app), integration (infra/web), functional (end-to-end flows).