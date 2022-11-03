using Microsoft.Extensions.Logging;

using Thoughts.Domain.Base.Entities;
using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.WebAPI.Clients.Blogs;

namespace Thoughts.UI.MAUI.Services
{
    public class BlogsManager : IBlogsManager
    {
        private readonly IBlogsService _blogsService;
        private readonly ILogger<BlogsManager> _logger;

        public BlogsManager(IBlogsService blogsService, ILogger<BlogsManager> logger)
        {
            _blogsService = blogsService;
            _logger = logger;
        }

        public async Task<IList<Post>> GetAllInfosAsync(CancellationToken cancellationToken = default)
        {
            var posts = await _blogsService.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return posts.ToList();
        }

        public IList<Post> GetAllInfos() => GetAllInfosAsync().GetAwaiter().GetResult();
    }
}
