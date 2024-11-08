using Microsoft.EntityFrameworkCore;

namespace Modular.Framework.Infrastructure.DataAccess;

public class UnitOfWork(DbContext context) : IUnitOfWork
// public class UnitOfWork(DbContext context, IDomainEventsDispatcher domainEventsDispatcher) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        // await domainEventsDispatcher.DispatchEventsAsync();
        return await context.SaveChangesAsync(cancellationToken);
    }
}

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}