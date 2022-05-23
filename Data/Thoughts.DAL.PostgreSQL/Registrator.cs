using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Thoughts.DAL.PostgreSQL
{
    public static class Registrator
    {
        public static IServiceCollection AddThoughtsDBPostgreSQL(this IServiceCollection services, string ConnectionString)
        {
            services.AddDbContext<ThoughtsDB>(opt => opt.UseNpgsql(ConnectionString, o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));

            return services;
        }
    }
}
