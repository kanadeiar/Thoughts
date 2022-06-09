using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Thoughts.DAL.Entities;
using Thoughts.DAL.Entities.DefaultData;

namespace Thoughts.DAL;

public class ThoughtsDB:DbContext
{
    #region DbSet

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    public DbSet<Post> Posts { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;

    #endregion

    public ThoughtsDB(DbContextOptions<ThoughtsDB> options) : base(options){ }

    protected override void ConfigureConventions(ModelConfigurationBuilder db)
    {
        base.ConfigureConventions(db);

        db.Properties<DateOnly>()
           .HaveConversion<DateOnlyConverter>();
        //.HaveColumnType("date");
    }

    protected override void OnModelCreating(ModelBuilder db)
    {
        base.OnModelCreating(db);

        db.Entity<Comment>()
           .HasOne(c => c.User)
           .WithMany(u => u.Comments)
           .OnDelete(DeleteBehavior.ClientNoAction);

        db.Entity<Comment>()
           .HasOne(c => c.Post)
           .WithMany(p => p.Comments)
           .OnDelete(DeleteBehavior.ClientNoAction);

        //db.Entity<Status>().HasData(GetDefaultData.DefaultStatus());
        //db.Entity<Role>().HasData(GetDefaultData.DefaultRole());

        //db.Entity<Post>()
        //   .HasOne(p => p.Status)
        //   .WithMany(s => s.Posts)
        //   .OnDelete(DeleteBehavior.NoAction);

        //db.Entity<Status>()
        //   .HasMany(s => s.Posts)
        //   .WithOne(p => p.Status)
        //   .OnDelete(DeleteBehavior.NoAction);
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(
            d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
        { }
    }
}
