using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Idetity;

namespace Thoughts.Identity.DAL
{
    public class IdentityDB : IdentityDbContext<IdentUser, IdentRole, string>
    {
        public IdentityDB(DbContextOptions<IdentityDB> options) : base(options) { }
    }
}