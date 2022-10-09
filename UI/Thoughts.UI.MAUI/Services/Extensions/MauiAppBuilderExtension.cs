using Microsoft.Extensions.Configuration;
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
            var webAPI = string.Empty;

#if DEBUG
            webAPI = DeviceInfo.Platform == DevicePlatform.Android
                ? settings.DebugWebAPIAndroid
                : settings.DebugWebAPI;

            services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();
            services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(webAPI))
                .AddTypedClient<IWeatherService, WeatherClient>()
                .ConfigurePrimaryHttpMessageHandler(provider => provider.GetHttpsMessageHandler());
#else
            webAPI = settings.DeviceWebAPI;
            services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(webAPI))
                .AddTypedClient<IWeatherService, WeatherClient>();
#endif

            services.AddSingleton(settings);
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

#if DEBUG
        private static HttpMessageHandler GetHttpsMessageHandler(this IServiceProvider services) => 
            services.GetRequiredService<IHttpsClientHandlerService>().GetPlatformMessageHandler();
#endif
    }
}
