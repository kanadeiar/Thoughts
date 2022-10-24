using Thoughts.UI.MAUI.ViewModels;

namespace Thoughts.UI.MAUI.Views;

public partial class BlogsPage : ContentPage
{
	public BlogsPage(BlogsViewModel blogsViewModel)
	{
		InitializeComponent();
        BindingContext = blogsViewModel;
	}
}