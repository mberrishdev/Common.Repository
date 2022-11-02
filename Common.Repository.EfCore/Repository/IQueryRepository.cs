using Common.Repository.EfCore.Sorting;
using System.Linq.Expressions;

namespace Common.Repository.EfCore.Repository
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, object>>[]? relatedProperties = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);
    }
}
