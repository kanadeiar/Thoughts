using System.Reflection;
using Microsoft.Extensions.Configuration;
using Thoughts.UI.MAUI.Services.Extensions;

namespace Thoughts.UI.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp App { get; private set; }

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var strAppConfigStreamName = string.Empty;

#if DEBUG
            strAppConfigStreamName = "Thoughts.UI.MAUI.appsettings.json";
#else
            strAppConfigStreamName = "Thoughts.UI.MAUI.appsettings.json";
#endif

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MauiProgram)).Assembly;
            using var stream = assembly.GetManifestResourceStream(strAppConfigStreamName);
            builder.Configuration.AddJsonStream(stream);

            builder.AddMAUIServices();

            return App = builder.Build();
        }
    }
}