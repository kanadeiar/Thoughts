using System.Net.Http.Json;

using Thoughts.Domain.Base.Entities;
using Thoughts.Interfaces.Base;

namespace Thoughts.WebAPI.Clients.Blogs
{
    public class BlogsClient : IBlogsService
    {
        private readonly HttpClient _httpClient;

        public BlogsClient(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<IEnumerable<Post>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var posts = await _httpClient
                .GetFromJsonAsync<IEnumerable<Post>>($"{WebApiControllersPath.BlogsUrl}/getallposts", cancellationToken)
                .ConfigureAwait(false);

            return posts ?? Enumerable.Empty<Post>();
        }

        public IEnumerable<Post> GetAll() => GetAllAsync().GetAwaiter().GetResult();
    }
}
