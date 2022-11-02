using Common.Repository.EfCore.UnitOfWork;
using Common.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Repository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfCoreDbContext<TDbContext>(this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder>? optionsAction = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            var contextType = typeof(TDbContext);
            if (AddedDbContext.GetContextType == contextType)
                return serviceCollection;


            serviceCollection.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);

            AddedDbContext.AddContextType<TDbContext>();
            return serviceCollection;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();

            return serviceCollection;
        }
    }
}
