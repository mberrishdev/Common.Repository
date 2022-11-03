using Common.Repository.EfCore.Options;
using Common.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace Common.Repository.EfCore.Repository
{
    public class EFCoreRepository<TDbContext, TEntity> : EfCoreQueryRepository<TDbContext, TEntity>, IRepository<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        public EFCoreRepository(TDbContext context, RepositoryOptions<TDbContext> repositoryOptions) : base(context, repositoryOptions)
        {
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            //TODO Exception
            if (entity == null)
                throw new ArgumentNullException();

            await Table.AddAsync(entity, cancellationToken);

            if (!_context.Entry(entity).IsKeySet)
                await _context.SaveChangesAsync(cancellationToken);

            await SaveChanges(cancellationToken);
            return entity;
        }


        private Task SaveChanges(CancellationToken cancellationToken)
        {
            if (_repositoryOptions.SaveChangeStrategy == SaveChangeStrategy.PerOperation)
                return _context.SaveChangesAsync(cancellationToken);

            if (_repositoryOptions.SaveChangeStrategy == SaveChangeStrategy.PerUnitOfWork
                && _context.Database.CurrentTransaction == null)
                return _context.SaveChangesAsync(cancellationToken);

            return Task.CompletedTask;
        }
    }
}
