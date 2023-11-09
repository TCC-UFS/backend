using PortalLegisAmbiental.Infrastructure;

namespace PortalLegisAmbiental.API.DependencyInjection
{
    internal static class Infrastructure
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<EfDbContext>();
        }
    }
}
