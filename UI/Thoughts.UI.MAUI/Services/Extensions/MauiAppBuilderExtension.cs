using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.UI.MAUI.ViewModels;
using Thoughts.UI.MAUI.Views;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.UI.MAUI.Services.Extensions
{
    public static class MauiAppBuilderExtension
    {
        public static MauiAppBuilder AddMAUIServices(this MauiAppBuilder builder)
        {
            builder.Services.AddMAUIServices(builder.Configuration);
            builder.Services.AddMAUIViews();
            builder.Services.AddMAUIViewModels();

            return builder;
        }

        public static IServiceCollection AddMAUIServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            var webAPI = DeviceInfo.Platform == DevicePlatform.Android
                ? settings.WebAPIAndroid
                : settings.WebAPI;

            services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();

#if DEBUG
            services.AddHttpClient("webAPI", client => client.BaseAddress = new Uri(webAPI))
                .ConfigurePrimaryHttpMessageHandler(provider => provider.GetHttpMessageHandler());
#else
            services.AddHttpClient("webAPI", client => client.BaseAddress = new Uri(webAPI));
#endif

            services.AddSingleton(settings);
            services.AddScoped<IWeatherService, WeatherClient>();
            services.AddScoped<IWeatherManager, WeatherManager>();

            return services;
        }

        public static IServiceCollection AddMAUIViews(this IServiceCollection services)
        {
            services.AddTransient<MainPage>();
            services.AddTransient<WeatherInfosPage>();

            return services;
        }

        public static IServiceCollection AddMAUIViewModels(this IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<WeatherViewModel>();

            return services;
        }

        private static HttpMessageHandler GetHttpMessageHandler(this IServiceProvider services) => 
            services.GetRequiredService<IHttpsClientHandlerService>().GetPlatformMessageHandler();
    }
}
