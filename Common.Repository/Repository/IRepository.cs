namespace Common.Repository.Repository
{
    public interface IRepository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
