using Thoughts.DAL.Sqlite;
using Thoughts.DAL.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddThoughtsDbSqlite(builder.Configuration.GetConnectionString("Sqlite"));
builder.Services.AddThoughtsDbSqlServer(builder.Configuration.GetConnectionString("SqlServer"));

var app = builder.Build();

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
