namespace Modular.Framework.Domain.DomainEvents;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTimeOffset OccurredOn { get; }
}