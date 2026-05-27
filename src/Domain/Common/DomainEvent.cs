using MediatR;

namespace Domain.Common;

public abstract record DomainEvent(DateTime OccurredOn) : INotification;