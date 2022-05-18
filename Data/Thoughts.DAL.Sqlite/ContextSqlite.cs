using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;
using Thoughts.DAL.Entities.DefaultData;

namespace Thoughts.DAL.Sqlite
{
    public class ContextSqlite:DbContext
    {
        #region DbSet
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        #endregion

        public ContextSqlite(DbContextOptions<ContextSqlite> options) : base(options) 
        {
            // Database.EnsureDeleted();
            // Database.EnsureCreated();
            //Database.Migrate(); 
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(GetDefaultData.DefaultStatus());
            modelBuilder.Entity<Role>().HasData(GetDefaultData.DefaultRole());
        }
    }
}
