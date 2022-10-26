using Thoughts.UI.MAUI.ViewModels;

namespace Thoughts.UI.MAUI.Views;

public partial class PostDetailsPage : ContentPage
{
	public PostDetailsPage(PostDetailsViewModel postDetailsViewModel)
	{
		InitializeComponent();
        BindingContext = postDetailsViewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}