namespace Thoughts.UI.MAUI.ViewModels
{
    internal class ViewModelLocator
    {
        public MainViewModel MainViewModel => MauiProgram.App.Services.GetRequiredService<MainViewModel>();

        public WeatherViewModel WeatherViewModel => MauiProgram.App.Services.GetRequiredService<WeatherViewModel>();
    }
}
