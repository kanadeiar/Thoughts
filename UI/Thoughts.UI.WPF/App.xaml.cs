using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureServices(ConfigureServices);
        
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {

        }
    }
}
