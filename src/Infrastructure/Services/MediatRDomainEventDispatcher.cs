using Application.Abstractions;
using Domain.Common;
using MediatR;

namespace Infrastructure.Services;

public sealed class MediatRDomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken ct = default)
    {
        foreach (var domainEvent in events)
            await publisher.Publish(domainEvent, ct);
    }
}