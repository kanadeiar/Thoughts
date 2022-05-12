using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Sqlite
{
    public class ContextSqlite:DbContext
    {
        #region DbSet
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Status> Statuss { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        #endregion

        private string connectionString= "Filename=ThoughtsData.db";
        public ContextSqlite()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        public ContextSqlite(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
            optionsBuilder              
            .UseSqlite(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
        }



        }
}
