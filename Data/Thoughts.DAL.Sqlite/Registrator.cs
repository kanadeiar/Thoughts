using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Thoughts.DAL.Sqlite;

public static class Registrator
{
    public static IServiceCollection AddThoughtsDbSqlite(this IServiceCollection services, string ConnectionString)
    {
        services.AddDbContext<ThoughtsDB>(opt => opt
            .UseSqlite(
                ConnectionString, 
                o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));

        return services;
    }

    //public static IServiceCollection AddThoughtsDbSqliteFactory(this IServiceCollection services, string ConnectionString)
    //{
    //    services.AddDbContextFactory<ThoughtsDB>(opt => opt
    //        .UseSqlite(
    //            ConnectionString, 
    //            o => o.MigrationsAssembly(typeof(Registrator).Assembly.FullName)));


    //    return services;
    //}
}
