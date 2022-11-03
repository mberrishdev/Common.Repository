using Microsoft.Extensions.DependencyInjection;
using Sample.Application.Rates;

namespace Sample.Application
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRateService, RateService>();

            return services;
        }
    }
}
