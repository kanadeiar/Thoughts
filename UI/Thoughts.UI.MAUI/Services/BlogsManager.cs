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

        public async Task<IEnumerable<Post>> GetAllInfosAsync(CancellationToken cancellationToken = default)
        {
            var result = default(IEnumerable<Post>);

            try
            {
                result = await _blogsService.GetAllAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method}: {message}", nameof(GetAllInfosAsync), e.Message);
            }

            return result ?? Enumerable.Empty<Post>();
        }

        public IEnumerable<Post> GetAllInfos() => GetAllInfosAsync().GetAwaiter().GetResult();
    }
}
