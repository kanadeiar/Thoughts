using Microsoft.EntityFrameworkCore;

using Thoughts.DAL;
using Thoughts.DAL.Sqlite;
using Thoughts.DAL.SqlServer;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

services.AddControllersWithViews();

var db_type = configuration["Database"];

switch (db_type)
{
    default: throw new InvalidOperationException($"Тип БД {db_type} не поддерживается");

    case "Sqlite":
        services.AddThoughtsDbSqlite(configuration.GetConnectionString("Sqlite"));
        break;

    case "SqlServer":
        services.AddThoughtsDbSqlServer(configuration.GetConnectionString("SqlServer"));
        break;
}

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    //var db = scope.ServiceProvider.GetRequiredService<ThoughtsDB>();
//    var db_factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ThoughtsDB>>();
//    using (var db = db_factory.CreateDbContext())
//    {
//        // выполнение операций над БД и его уничтожение
//    }
//}

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
