using System.Reflection;
using Modular.Framework.Application.Events;
using Modular.Framework.Domain.DomainEvents;
using Modular.Framework.Infrastructure.Mediator;

namespace Modular.Framework.Infrastructure.DomainEvents;

public class DomainEventsDispatcher(IMediator mediator) : IDomainEventsDispatcher
{
    public async Task Dispatch(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var de in domainEvents)
        {
            var domainEventType = de.GetType();
            var mediatorDomainEventTypeOpenGeneric = typeof(DomainEvent<>);
            var mediatorDomainEventType = mediatorDomainEventTypeOpenGeneric.MakeGenericType(domainEventType);
            var mediatorDomainEvent = Activator.CreateInstance(mediatorDomainEventType, de.Id, de);

            // TODO add caching of this search
            await (Task) this.GetType().GetMethod(nameof(MediatorHandle), BindingFlags.NonPublic | BindingFlags.Instance)!
                             .MakeGenericMethod(domainEventType)
                             .Invoke(this, [mediatorDomainEvent, cancellationToken])!;
        }
    }

    private async Task MediatorHandle<TDomainEvent>(DomainEvent<TDomainEvent> domainEvent, CancellationToken cancellationToken) where TDomainEvent : IDomainEvent
    {
        await mediator.Handle(domainEvent, cancellationToken);
    }
}

public interface IDomainEventsDispatcher
{
    Task Dispatch(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken);
}