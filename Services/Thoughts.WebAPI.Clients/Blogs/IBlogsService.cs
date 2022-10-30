using Thoughts.Domain.Base.Entities;

namespace Thoughts.WebAPI.Clients.Blogs
{
    public interface IBlogsService
    {
        Task<IEnumerable<Post>> GetAllAsync(CancellationToken token = default);

        IEnumerable<Post> GetAll();
    }
}
