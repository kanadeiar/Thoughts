using Thoughts.UI.MAUI.ViewModels;

namespace Thoughts.UI.MAUI.Views;

public partial class PostDetailsPage : ContentPage
{
	public PostDetailsPage(PostDetailsViewModel postDetailsViewModel)
	{
		InitializeComponent();
        BindingContext = postDetailsViewModel;
    }
}