var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;


services.AddControllersWithViews().AddRazorRuntimeCompilation();

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

services.AddTransient<ThoughtsDbInitializer>();
services.AddScoped<IBlogPostManager, SqlBlogPostManager>();

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
