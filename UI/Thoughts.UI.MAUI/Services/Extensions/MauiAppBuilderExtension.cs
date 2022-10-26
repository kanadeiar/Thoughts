using Microsoft.Extensions.Configuration;
using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.UI.MAUI.ViewModels;
using Thoughts.UI.MAUI.Views;
using Thoughts.WebAPI.Clients.Blogs;
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
                .AddTypedClient<IBlogsService, BlogsClient>()
                .ConfigurePrimaryHttpMessageHandler(provider => provider.GetHttpsMessageHandler());
#else
            webAPI = settings.DeviceWebAPI;
            services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(webAPI))
                .AddTypedClient<IWeatherService, WeatherClient>()
                .AddTypedClient<IBlogsService, BlogsClient>();
#endif

            services.AddSingleton(settings);
            services.AddSingleton<IWeatherManager, WeatherManager>();
            services.AddSingleton<IBlogsManager, BlogsManager>();

            return services;
        }

        public static IServiceCollection AddMAUIViews(this IServiceCollection services)
        {
            services.AddSingleton<MainPage>();
            services.AddSingleton<WeatherInfosPage>();
            services.AddSingleton<BlogsPage>();

            services.AddTransient<PostDetailsPage>();

            return services;
        }

        public static IServiceCollection AddMAUIViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<WeatherViewModel>();
            services.AddSingleton<BlogsViewModel>();

            services.AddTransient<PostDetailsViewModel>();

            return services;
        }

#if DEBUG
        private static HttpMessageHandler GetHttpsMessageHandler(this IServiceProvider services) => 
            services.GetRequiredService<IHttpsClientHandlerService>().GetPlatformMessageHandler();
#endif
    }
}
