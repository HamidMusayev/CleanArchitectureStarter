using Domain.Common;

namespace Domain.Users.Events;

public sealed record UserUpdatedDomainEvent(Guid UserId, DateTime UpdatedAt)
    : DomainEvent(UpdatedAt);
