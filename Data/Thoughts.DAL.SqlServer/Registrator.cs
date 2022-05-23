using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Thoughts.DAL.SqlServer
{
    public static class Registrator
    {
        public static IServiceCollection AddThoughtsDBSqlServer(this IServiceCollection services, string ConnectionString)
        {
            services.AddDbContext<ThoughtsDB>(opt => opt.UseSqlServer(ConnectionString, o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));

            return services;
        }
    }
}
