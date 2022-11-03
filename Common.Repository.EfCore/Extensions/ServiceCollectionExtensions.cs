using Common.Repository.EfCore.Options;
using Common.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Repository.EfCore.Exceptions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCoreDbContext<TDbContext>(this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder>? optionsAction = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped,
            Action<RepositoryOptions<TDbContext>>? repositoryOptions = null)
            where TDbContext : DbContext
        {
            var contextType = typeof(TDbContext);
            if (AddedDbContext.GetContextType == contextType)
                return serviceCollection;


            serviceCollection.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);
            AddedDbContext.AddContextType<TDbContext>();

            var repoOpts = new RepositoryOptions<TDbContext>();
            repositoryOptions?.Invoke(repoOpts);
            serviceCollection.AddSingleton(repoOpts);

            //ToDo add repository

            return serviceCollection;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

            return serviceCollection;
        }
    }
}
