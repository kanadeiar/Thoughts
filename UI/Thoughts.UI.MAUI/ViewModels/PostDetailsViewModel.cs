using Thoughts.Domain.Base.Entities;
using Thoughts.UI.MAUI.ViewModels.Base;

namespace Thoughts.UI.MAUI.ViewModels
{
    [QueryProperty("Post", "Post")]
    public class PostDetailsViewModel : ViewModel
    {
        private Post _post;

        public Post Post
        {
            get => _post;

            set => Set(ref _post, value);
        }
    }
}
