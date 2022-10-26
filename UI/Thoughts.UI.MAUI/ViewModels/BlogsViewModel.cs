using System.Collections.ObjectModel;
using System.Windows.Input;

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

        #endregion

        #region Constructors

        public BlogsViewModel(IBlogsManager blogsManager)
        {
            _blogsManager = blogsManager;
        } 

        #endregion

        #region Bindable properties

        public ObservableCollection<Post> Posts { get; } = new();

        private string _title = "Blogs";

        public string Title { get => _title; set => Set(ref _title, value); }

        #endregion

        #region Commands

        #region Refresh

        private ICommand _refreshCommand;

        public ICommand RefreshCommand => _refreshCommand ??= new Command(RefreshDataAsync);

        private async void RefreshDataAsync()
        {
            Posts.Clear();

            var posts = await _blogsManager.GetAllInfosAsync();

            foreach (var post in posts)
                Posts.Add(post);
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
