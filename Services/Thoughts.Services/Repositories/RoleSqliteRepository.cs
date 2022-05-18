using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Thoughts.DAL.Entities.Base;
using Thoughts.DAL.Sqlite;

namespace Thoughts.Services.Repositories;

public class RoleSqlRepository<T> : BaseSqlRepository<T> where T : Role
{
    public RoleSqlRepository(ContextSqlite dbContextSql, ILogger<RoleSqlRepository<T>> logger) : base(dbContextSql, logger)
    {
    }
}