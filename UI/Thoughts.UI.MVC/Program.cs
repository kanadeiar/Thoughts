using Thoughts.Interfaces.Base;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services.Mapping;
using Thoughts.UI.MVC.Infrastructure.AutoMapper;
using Thoughts.WebAPI.Clients.ShortUrl;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;


services.AddControllersWithViews();//.AddRazorRuntimeCompilation();
var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

// To list physical files from a path provided by configuration:
var uploadFileOptions = configuration.GetSection("UploadFileOptions");

var shared = new SharedConfiguration()
{
    FileSizeLimit = uploadFileOptions.GetValue<long>("FileSizeLimit"),
    TargetFilePath = Path.Combine(contentRoot + "upload\\"),//uploadFileOptions.GetValue<string>("StoredFilesPath"),
    PermittedExtensionsForUploadedFile = uploadFileOptions
        .GetSection("PermittedExtensions")?
        .GetChildren()?.Select(i => i.Value)?.ToArray()
};
services.AddSingleton(shared);

var db_type = configuration["Database"];

switch (db_type)
{
    default: throw new InvalidOperationException($"Тип БД {db_type} не поддерживается");

    case "Sqlite":
        services.AddThoughtsDbSqlite(configuration.GetConnectionString("Sqlite"));
        break;

    case "SqlServer":
        services.AddThoughtsDbSqlServer(configuration.GetConnectionString("SqlServer"));
        services.AddFileStorageDbSqlServer(configuration.GetConnectionString("FileStorageServer"));
        break;
}
services.AddIdentityDBSqlServer(configuration.GetConnectionString("IdentitySqlServer"));

services.AddTransient<ThoughtsDbInitializer>();
services.AddTransient<IdentityDbInitializer>();
services.AddScoped<IBlogPostManager, SqlBlogPostManager>();
services.AddTransient<IShortUrlManager, ShortUrlClient>();
services.AddScoped<IFileManager, FileStorageManager>();
services.AddAutoMapper(typeof(BlogDetailsWebModelProfile));
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
        name: "shortUrl",
        pattern: "url/{Alias?}",
        defaults: new { controller = "ShortUrl", action = "GetUrl" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
