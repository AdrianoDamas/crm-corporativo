### Quick context

This is a small .NET 8 solution implementing a domain-driven CRM core and an infrastructure project.

- Solution: `crm-corporativo.sln`
- Domain project: `src/CRM.Domain` (entities, value objects, domain events, core primitives)
- Infrastructure project: `src/CRM.Infrastructure` (EF Core packages referenced; persistence/Repositories folder scaffolded)

## What an AI assistant should know (short)

- The codebase follows simple DDD patterns: `Entity`, `AggregateRoot`, and `DomainEvent` live in `CRM.Domain/Core`.
- Domain behaviors are implemented on aggregates (see `Customer.CreateIndividual`, `Customer.Deactivate`) and raise domain events (`CustomerCreatedEvent`, `CustomerDeactivatedEvent`).
- Persistence implementations are expected in `CRM.Infrastructure` and should implement domain interfaces like `CRM.Domain.Interfaces.ICustomerRepository`.
- EF Core (Postgres provider) is referenced in the infrastructure project (`Npgsql.EntityFrameworkCore.PostgreSQL`). The actual DbContext and repository implementations are not present (folders `Persistence/` and `Repositories/` are empty scaffolds).

## Typical tasks and how to approach them

- Add a repository implementation: implement `ICustomerRepository` inside `src/CRM.Infrastructure/Repositories`. Match method signatures exactly (async Task patterns). See `ICustomerRepository` for contract.
- Implement DbContext: add an EF Core DbContext under `src/CRM.Infrastructure/Persistence` and register it with Postgres provider. Use `Microsoft.EntityFrameworkCore` 8.0 APIs.
- When changing aggregates, check for domain events: aggregates store events via `AggregateRoot.RaiseDomainEvent` and expose `DomainEvents` (clear them after dispatching). Keep event constructors calling `base(aggregateId)`.

## Build, run and test notes

- The repo uses .NET 8. Build from solution root:

  dotnet build crm-corporativo.sln

- Docker compose is mentioned in the top-level README; if used, prefer running the compose at repo root: `docker-compose up` (the compose file is not in repository — confirm with the maintainer if needed).

## Patterns and conventions to follow

- Use UTC for timestamps: entities set `CreatedAt = DateTime.UtcNow` and DomainEvent uses `DateTime.UtcNow` for `OccurredAt`.
- Entities generate Ids on construction (Guid.NewGuid()). Preserve this behavior when persisting (avoid overwriting Id unless explicit migration).
- Prefer factory-style static constructors on the aggregate (e.g., `Customer.CreateIndividual` / `CreateCompany`) for creating valid aggregates and raising events.
- Throw domain-specific exceptions (ArgumentException, InvalidOperationException) inside factories to guard invariants — follow existing style.

## When modifying persistence or DI

- Add project reference to `CRM.Domain` when implementing repositories (already present in `CRM.Infrastructure.csproj`).
- Keep EF Core package versions aligned with `CRM.Infrastructure.csproj` (currently 8.0.0). Do not upgrade packages without running the build.

## Files worth checking when editing behavior

- `src/CRM.Domain/Core/AggregateRoot.cs` — domain event storage pattern
- `src/CRM.Domain/Core/Entity.cs` — Id and timestamp conventions
- `src/CRM.Domain/Entities/Customer.cs` — real-world examples of factories, validation, and raising events
- `src/CRM.Domain/Events/*.cs` — event payloads and base `DomainEvent` contract
- `src/CRM.Infrastructure/CRM.Infrastructure.csproj` — package versions and project references

## Examples (copyable snippets)

- Implement repository signature (paste into `CRM.Infrastructure/Repositories/CustomerRepository.cs`):

```csharp
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;

public class CustomerRepository : ICustomerRepository
{
    // implement GetByIdAsync, AddAsync, UpdateAsync, SaveChangesAsync
}
```

## Do not guess — ask the maintainer when:

- Docker compose file, database connection strings, or secrets are required but not present in the repository.
- You need to decide on migrations, schema names, or existing production hosting details.

## Closing

If something here is ambiguous (e.g., expected DbContext shape or how domain events are dispatched), ask the maintainer for the preferred event dispatcher and database connection details before implementing.

---
Please review and tell me if you'd like more details (example DbContext, DI registration, or a sample repository implementation).
