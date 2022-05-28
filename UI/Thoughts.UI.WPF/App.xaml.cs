using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Thoughts.DAL.Sqlite;
using Thoughts.DAL.SqlServer;
using Thoughts.UI.WPF.ViewModels;

namespace Thoughts.UI.WPF
{
    public partial class App
    {
        private static IHost? __Hosting;
        
        public static IHost Hosting => __Hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        
        public static IServiceProvider Services => Hosting.Services;

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();

            services.AddThoughtsDbSqlite(host.Configuration.GetConnectionString("Sqlite"));
            //services.AddThoughtsDbSqlServer(host.Configuration.GetConnectionString("SqlServer"));
        }

    }
}
