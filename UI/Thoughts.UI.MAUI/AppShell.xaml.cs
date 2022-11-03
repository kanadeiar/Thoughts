using Thoughts.UI.MAUI.Views;

namespace Thoughts.UI.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PostDetailsPage), typeof(PostDetailsPage));
        }
    }
}