using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services.Mapping;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;


services.AddControllersWithViews();//.AddRazorRuntimeCompilation();

var db_type = configuration["Database"];

switch (db_type)
{
    default: throw new InvalidOperationException($"Тип БД {db_type} не поддерживается");

    case "Sqlite":
        services.AddThoughtsDbSqlite(configuration.GetConnectionString("Sqlite"));
        break;

    case "SqlServer":
        services.AddThoughtsDbSqlServer(configuration.GetConnectionString("SqlServer"));
        services.AddIdentityDBSqlServer(configuration.GetConnectionString("IdentitySqlServer"));
        break;
}

services.AddTransient<ThoughtsDbInitializer>();
services.AddTransient<IdentityDbInitializer>();
services.AddScoped<IBlogPostManager, SqlBlogPostManager>();

//services.AddScoped<IRepository<Post>, MappingRepository<Thoughts.DAL.Entities.Post, Post>>();
//services.AddScoped<IRepository<Category>, MappingRepository<Thoughts.DAL.Entities.Category, Category>>();
//services.AddScoped<IRepository<Tag>, MappingRepository<Thoughts.DAL.Entities.Tag, Tag>>();
//services.AddScoped<IRepository<Comment>, MappingRepository<Thoughts.DAL.Entities.Comment, Comment>>();

//services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));

var app = builder.Build();

await app.InitializeDatabase();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
