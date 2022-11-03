using Common.Repository.EfCore.Pagination;
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
            int? skip = null, int? take = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();
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

        public async Task<PagedList<TEntity>> GetListByPageAsync(int pageIndex, int pageSize,
            Expression<Func<TEntity, object>>[]? relatedProperties = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (relatedProperties != null)
                query = relatedProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.ASC)
                query = query.OrderBy(sortingDetails.SortItem.SortBy);

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.DESC)
                query = query.OrderByDescending(sortingDetails.SortItem.SortBy);


            return await query.Paginate<TEntity>(pageIndex, pageSize, cancellationToken);
        }

        public Task<TEntity> GetAsync(object key, Expression<Func<TEntity, object>>[]? relatedProperties = null, CancellationToken cancellationToken = default)
        {
            //TODO_RE
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>>[]? relatedProperties = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (relatedProperties != null)
                query = relatedProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.AnyAsync(cancellationToken);
        }
    }
}
