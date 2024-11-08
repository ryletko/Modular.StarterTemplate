using Microsoft.EntityFrameworkCore;

namespace Example.ReadModel.Context;

internal class DataQuery(ReadDbContext dbContext) : IDataQuery
{
    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return dbContext.Set<TEntity>().AsNoTracking();
    }
}