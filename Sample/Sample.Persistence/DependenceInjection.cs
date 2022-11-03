using Common.Repository.EfCore.Exceptions;
using Common.Repository.EfCore.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sample.Persistence
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEfCoreDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AppContextConnection"));
            },
            repositoryOptions: options =>
            {
                options.SaveChangeStrategy = SaveChangeStrategy.PerOperation;
            });

            services.AddUnitOfWork();

            return services;
        }
    }
}
