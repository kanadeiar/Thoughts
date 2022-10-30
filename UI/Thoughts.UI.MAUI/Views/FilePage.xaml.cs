using Thoughts.UI.MAUI.ViewModels;

namespace Thoughts.UI.MAUI.Views;

public partial class FilePage : ContentPage
{
    public FilePage(FileViewModel fileViewModel)
	{
		InitializeComponent();
        BindingContext = fileViewModel;
    }
}