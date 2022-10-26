using System.Collections.ObjectModel;
using System.Windows.Input;

using Microsoft.Extensions.Logging;

using Thoughts.Domain.Base.Entities;
using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.UI.MAUI.ViewModels.Base;
using Thoughts.UI.MAUI.Views;

namespace Thoughts.UI.MAUI.ViewModels
{
    public class BlogsViewModel : ViewModel
    {
        #region Fields

        private readonly IBlogsManager _blogsManager;
        private readonly IConnectivity _connectivity;
        private readonly ILogger<BlogsViewModel> _logger;

        #endregion

        #region Constructors

        public BlogsViewModel(IBlogsManager blogsManager, 
            IConnectivity connectivity, 
            ILogger<BlogsViewModel> logger)
        {
            _blogsManager = blogsManager;
            _connectivity = connectivity;
            _logger = logger;

            Title = "Blogs";
        } 

        #endregion

        #region Bindable properties

        public ObservableCollection<Post> Posts { get; } = new();

        private bool _isRefresh;

        public bool IsRefreshing
        {
            get => _isRefresh;
            set => Set(ref _isRefresh, value);
        }

        #endregion

        #region Commands

        #region Refresh

        private ICommand _refreshCommand;

        public ICommand RefreshCommand => _refreshCommand ??= new Command(RefreshDataAsync);

        private async void RefreshDataAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if(_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    _logger.LogError("{Method}: {message}", nameof(RefreshDataAsync), "Check your internet connection");
                    await Shell.Current.DisplayAlert("Internet connection failed!",
                        $"Unable to get blogs: Check your internet connection", "OK");
                }

                IsBusy = true;

                var posts = await _blogsManager.GetAllInfosAsync();

                if(posts is { Count: > 0})
                    Posts.Clear();

                foreach (var post in posts)
                    Posts.Add(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method}: {message}", nameof(RefreshDataAsync), ex.Message);
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to get blogs: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        #endregion

        #region Go to post details

        private ICommand _goToPostDetailsCommand;

        public ICommand GoToPostDetailsCommand => _goToPostDetailsCommand ??= new Command(GoToPostDetailsAsync);

        private async void GoToPostDetailsAsync(object obj)
        {
            if (obj is null && obj is not Post)
                return;

            var post = (Post)obj;

            await Shell.Current.GoToAsync($"{nameof(PostDetailsPage)}", true,
                new Dictionary<string, object>
                {
                    { "Post", post}
                });
        }

        #endregion

        #endregion
    }
}
