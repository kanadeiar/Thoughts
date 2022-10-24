using Thoughts.UI.MAUI.ViewModels;

namespace Thoughts.UI.MAUI.Views;

public partial class WeatherInfosPage : ContentPage
{
    public WeatherInfosPage(WeatherViewModel weatherViewModel)
	{
		InitializeComponent();
        BindingContext = weatherViewModel;
    }
}