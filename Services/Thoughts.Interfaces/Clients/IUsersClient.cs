using Microsoft.AspNetCore.Identity;

using Thoughts.DAL.Entities.Idetity;

namespace Thoughts.Interfaces.Clients
{
    public interface IUsersClient :
    IUserRoleStore<IdentUser>,
    IUserPasswordStore<IdentUser>,
    IUserEmailStore<IdentUser>,
    IUserPhoneNumberStore<IdentUser>,
    IUserTwoFactorStore<IdentUser>,
    IUserLoginStore<IdentUser>,
    IUserClaimStore<IdentUser>
    {

    }
}
