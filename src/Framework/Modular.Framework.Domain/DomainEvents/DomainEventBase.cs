using Modular.Framework.Domain.DomainContext;

namespace Modular.Framework.Domain.DomainEvents;

public record DomainEventBase : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DomainContextAccessor.Current.StartedAt;
}