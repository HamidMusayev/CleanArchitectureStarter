using Domain.Common;

namespace Domain.Users.Events;

public sealed record UserDeletedDomainEvent(Guid UserId, DateTime DeletedAt)
    : DomainEvent(DeletedAt);
