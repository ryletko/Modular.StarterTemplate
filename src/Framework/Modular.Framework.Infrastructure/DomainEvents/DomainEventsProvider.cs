using Microsoft.EntityFrameworkCore;
using Modular.Framework.Domain.DomainEvents;
using Modular.Framework.Domain.Patterns;

namespace Modular.Framework.Infrastructure.DomainEvents;

public class DomainEventsProvider(DbContext dbContext) : IDomainEventsProvider
{
    public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
    {
        var domainEntities = dbContext.ChangeTracker
                                      .Entries<Entity>()
                                      .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        return domainEntities
              .SelectMany(x => x.Entity.DomainEvents)
              .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = dbContext.ChangeTracker
                                      .Entries<Entity>()
                                      .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
    }
}