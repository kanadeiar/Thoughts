using Serilog;
namespace Thoughts.WebAPI;

public static class Program
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((context, loggerConfiguration) => 
                loggerConfiguration.ReadFrom.Configuration(context.Configuration))
            .ConfigureWebHostDefaults(webHostBuilder => 
                webHostBuilder.UseStartup<Startup>());
}