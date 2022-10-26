using Thoughts.Domain.Base.Entities;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.UI.MAUI.Services.Interfaces
{
    public interface IBlogsManager
    {
        Task<IList<Post>> GetAllInfosAsync(CancellationToken cancellationToken = default);

        IList<Post> GetAllInfos();
    }
}
