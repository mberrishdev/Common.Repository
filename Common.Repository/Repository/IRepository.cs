using Common.Repository.Lists.Sorting;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.Repository.Repository
{
    public interface IRepository<TEntity> : IQueryRepository<TEntity>
        where TEntity : class
    {
        Task<List<TEntity>> GetListForUpdateAsync(List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            int? skip = null, int? take = null,
            CancellationToken cancellationToken = default);

        Task<TEntity> GetForUpdateAsync(Expression<Func<TEntity, bool>> predicate,
           List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
           CancellationToken cancellationToken = default);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
