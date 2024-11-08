using Modular.Framework.Domain.DomainContext;
using Modular.Framework.Domain.DomainEvents;
using Modular.Framework.Domain.Identifiers;

namespace Modular.Framework.Domain.Patterns;

public abstract class Entity
{
    private List<IDomainEvent> _domainEvents;

    /// <summary>
    /// Domain events occurred.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }

    /// <summary>
    /// Add domain event.
    /// </summary>
    /// <param name="domainEvent">Domain event.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= [];

        this._domainEvents.Add(domainEvent);
    }
}

public abstract class EntityAudited<TId> : Entity where TId : TypedIdValueBase
{
    public TId Id { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    protected EntityAudited()
    {
    }

    protected EntityAudited(bool isNew)
    {
        if (isNew)
        {
            Id        = (TId) Activator.CreateInstance(typeof(TId), Guid.NewGuid());
            CreatedBy = DomainContextAccessor.Current.UserId;
            CreatedAt = DomainContextAccessor.Current.StartedAt;
        }
    }
}