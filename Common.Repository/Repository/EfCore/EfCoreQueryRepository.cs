﻿using Common.Repository.EfCore.Options;
using Common.Repository.Lists.Pagination;
using Common.Repository.Lists.Sorting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.Repository.Repository.EfCore
{
    public class EfCoreQueryRepository<TDbContext, TEntity> : IQueryRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        #region ProtectedMembers
        protected readonly TDbContext _context;
        protected readonly RepositoryOptions<TDbContext> _repositoryOptions;
        protected DbSet<TEntity> Table => _context.Set<TEntity>();
        #endregion

        #region ctor
        public EfCoreQueryRepository(TDbContext context, RepositoryOptions<TDbContext> repositoryOptions)
        {
            _context = context;
            _repositoryOptions = repositoryOptions;
        }
        #endregion

        public async Task<List<TEntity>> GetListAsync(List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            int? skip = null, int? take = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (relatedProperties != null)
                query = relatedProperties.Aggregate(query, (current, include) => include(current));

            return await GetListAsync(query, predicate, sortingDetails, skip, take, cancellationToken);
        }

        public async Task<PagedList<TEntity>> GetListByPageAsync(int pageIndex, int pageSize,
                        Expression<Func<TEntity, bool>>? predicate = null,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            SortingDetails<TEntity>? sortingDetails = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (relatedProperties != null)
                query = relatedProperties.Aggregate(query, (current, include) => include(current));

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.ASC)
                query = query.OrderBy(sortingDetails.SortItem.SortBy);

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.DESC)
                query = query.OrderByDescending(sortingDetails.SortItem.SortBy);

            var count = await query.CountAsync(cancellationToken: cancellationToken);
            var pagingDetails = new PagingDetails(pageIndex, pageSize, count);

            if (count == 0)
                return new PagedList<TEntity>(new List<TEntity>(), pagingDetails.PageIndex, pagingDetails.PageSize, count);

            var list = await query.Skip(pagingDetails.PageIndex * pagingDetails.PageSize)
                                  .Take(pagingDetails.PageSize)
                                  .ToListAsync(cancellationToken: cancellationToken);

            return new PagedList<TEntity>(list, pagingDetails);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? relatedProperties = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = Table.AsNoTracking();

            if (relatedProperties != null)
                query = relatedProperties.Aggregate(query, (current, include) => include(current));

            return await query.FirstOrDefaultAsync(predicate,
                                                   cancellationToken);
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

        #region ProtectedMethods
        protected async Task<List<TEntity>> GetListAsync(IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>>? predicate = null,
            SortingDetails<TEntity>? sortingDetails = null,
            int? skip = null, int? take = null,
            CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                source = source.Where(predicate);

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.ASC)
                source = source.OrderBy(sortingDetails.SortItem.SortBy);

            if (sortingDetails?.SortItem?.SortBy != null && sortingDetails.SortItem.SortDirection == SortDirection.DESC)
                source = source.OrderByDescending(sortingDetails.SortItem.SortBy);

            if (skip is not null && skip is > 0)
                source = source.Skip(skip.Value);

            if (take is not null && take is > 0)
                source = source.Take(take.Value);

            return await source.ToListAsync(cancellationToken);
        }
        #endregion
    }
}
