using Common.Repository.Lists.Pagination;
using Common.Repository.Lists.Sorting;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.Repository.Repository
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetListAsync(List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default);

        Task<PagedList<TEntity>> GetListByPageAsync(int pageIndex, int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            SortingDetails<TEntity>? sortingDetails = null,
            CancellationToken cancellationToken = default);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            CancellationToken cancellationToken = default);

        Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default);
    }
}
