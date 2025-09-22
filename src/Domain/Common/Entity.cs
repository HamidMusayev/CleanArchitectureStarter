namespace Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    private readonly List<DomainEvent> _events = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _events;

    protected void Raise(DomainEvent e) => _events.Add(e);
    public void ClearEvents() => _events.Clear();
}