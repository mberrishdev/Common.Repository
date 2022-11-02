using Common.Repository.EfCore.Repository;
using Common.Repository.EfCore.Sorting;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Repository.Repository
{
    public class EfCoreQueryRepository<TDbContext, TEntity> : IQueryRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        private readonly TDbContext _context;
        protected DbSet<TEntity> Table => _context.Set<TEntity>();

        public EfCoreQueryRepository(TDbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, object>>[]? relatedProperties = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            int? skip = null,
            int? take = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table;
            if (predicate != null)
                query = query.Where(predicate);

            if (relatedProperties != null)
                query = relatedProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.ASC)
                query = query.OrderBy(sortingDetails.SortItem.SortBy);

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.DESC)
                query = query.OrderByDescending(sortingDetails.SortItem.SortBy);

            if (skip is not null && skip is > 0)
                query = query.Skip(skip.Value);

            if (take is not null && take is > 0)
                query = query.Take(take.Value);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
