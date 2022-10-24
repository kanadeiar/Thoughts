using Thoughts.Domain.Base.Entities;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.UI.MAUI.Services.Interfaces
{
    public interface IBlogsManager
    {
        Task<IEnumerable<Post>> GetAllInfosAsync(CancellationToken cancellationToken = default);

        IEnumerable<Post> GetAllInfos();
    }
}
