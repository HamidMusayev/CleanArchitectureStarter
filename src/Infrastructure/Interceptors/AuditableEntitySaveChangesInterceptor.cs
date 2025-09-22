using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Interceptors;

public sealed class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Apply auditing logic here if needed.
        return base.SavingChanges(eventData, result);
    }
}