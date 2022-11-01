using System;
using System.Net.Http;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Polly;
using Polly.Extensions.Http;

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

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<FilesViewModel>();
            services.AddSingleton<RecordsViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<AccountsViewModel>();

            services.AddHttpClient("ThoughtsAPI", http => http.BaseAddress = new(host.Configuration["WebAPI:Url"]))
                .AddTypedClient<AccountClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000)
            {
                var jitter = new Random();
                return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .WaitAndRetryAsync(MaxRetryCount, RetryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) +
                        TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
            }

            static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
                HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));
        }
    }
}
