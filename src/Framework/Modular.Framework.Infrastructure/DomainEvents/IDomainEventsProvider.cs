using Modular.Framework.Domain.DomainEvents;

namespace Modular.Framework.Infrastructure.DomainEvents;

public interface IDomainEventsProvider
{
    IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();
    void ClearAllDomainEvents();
}