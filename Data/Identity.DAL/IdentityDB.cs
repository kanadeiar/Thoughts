using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Idetity;

namespace Identity.DAL
{
    public class IdentityDB : IdentityDbContext<User, Role, string>
    {
        public IdentityDB(DbContextOptions<IdentityDB> options) : base(options) { }
    }
}