using Identity.DAL;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Thoughts.DAL.Entities.Idetity;
using Thoughts.DAL.Sqlite;
using Thoughts.DAL.SqlServer;
using Thoughts.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

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
services.AddIdentityDBSqlServer(configuration.GetConnectionString("IdentitySqlServer"));

services.AddTransient<ThoughtsDbInitializer>();
services.AddTransient<IdentityDbInitializer>();

services.AddIdentity<User, Role>()
   .AddEntityFrameworkStores<IdentityDB>()
   .AddDefaultTokenProviders();

services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif
    //opt.User.RequireUniqueEmail = false; // уникальные email
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";
    opt.Lockout.AllowedForNewUsers = false; // блокировка новых пользователей
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

// нужна ли настройка Cookie?
//services.ConfigureApplicationCookie(opt =>
//{
//    opt.Cookie.Name = "Thoughts.WebAPI";
//    opt.Cookie.HttpOnly = true;

//    opt.ExpireTimeSpan = TimeSpan.FromDays(10);

//    opt.LoginPath = "/Account/Login";
//    opt.LogoutPath = "/Account/Logout";
//    opt.AccessDeniedPath = "/Account/AccessDenied";

//    opt.SlidingExpiration = true;
//});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    //endpoints.MapControllerRoute(
//    //    name: "default",
//    //    pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.Run();

public partial class Program { }
