using Microsoft.EntityFrameworkCore;
using Serilog;

using Thoughts.DAL.Entities.Base;
using Thoughts.DAL.Sqlite;
using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services.Repositories;


namespace Thoughts.WebAPI;

public class Startup
{
    private const string ConnectionString = "Filename=../../ThoughtsData.db";
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<ContextSqlite>(builder =>
            builder.UseSqlite(ConnectionString));
        services.AddScoped<IRepository<Role>, RoleSqlRepository<Role>>();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (_env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI(o => o.SwaggerEndpoint(@"/swagger/v1/swagger.json", "WebApi"));
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}