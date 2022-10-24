using Microsoft.AspNetCore.Identity;

using Thoughts.DAL.Entities.Idetity;

namespace Thoughts.Interfaces.Clients
{
    public interface IRolesClient : IRoleStore<IdentRole>
    {

    }
}
