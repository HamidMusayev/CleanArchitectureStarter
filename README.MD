# Clean Architecture Starter

### Instructions
Use `dotnet build` or the CI pipeline. Add custom scripts here.

### Prerequisites
- .NET 9.0 SDK or later

### References
- **Api** references: `Application`, `Contracts`, `Infrastructure`
- **Application** references: `Domain`, `Contracts`
- **Infrastructure** references: `Application`, `Domain`
- **Contracts** no project references
- **SharedKernel** referenced by `Domain` (and others if needed)

## Features
- [x] Clean architecture
- [x] Global exception filter & middleware
- [x] Api versioning
- [x] Serilog
- [x] Swagger
- [x] Entity Framework Core
- [x] Dependency injection
- [x] CQRS pattern
- [x] MediatR

