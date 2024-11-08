using Modular.Framework.Application.Message;

namespace Modular.Framework.Application.Events;

public class DomainEvent<T>(Guid id,
                            T domainEvent) : IMessage
{
    public Guid Id { get; } = id;

    public T Value { get; } = domainEvent;
}