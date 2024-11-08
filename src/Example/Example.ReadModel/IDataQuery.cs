namespace Example.ReadModel;

public interface IDataQuery
{
    public IQueryable<TEntity> Query<TEntity>() where TEntity : class;
}