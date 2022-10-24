using System;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Thoughts.UI.WPF.ViewModels;
using Thoughts.WebAPI.Clients.Identity;

namespace Thoughts.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost? __Hosting;
        public static IHost Hosting => __Hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static IServiceProvider Services => Hosting.Services;        

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(opt => opt.AddJsonFile("appconfig.json", false, true)).ConfigureServices(ConfigureServices);
        
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<FilesViewModel>();
            services.AddSingleton<RecordsViewModel>();
            services.AddSingleton <MainWindowViewModel>();
            services.AddSingleton<AccountClient>();
            
        }
    }
}
