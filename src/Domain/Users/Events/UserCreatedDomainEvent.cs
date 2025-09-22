using Domain.Common;

namespace Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId, string Email, DateTime CreatedAt)
    : DomainEvent(DateTime.UtcNow);