using Microsoft.AspNetCore.Identity;

namespace Thoughts.DAL.Entities.Idetity
{
    public class IdentUser : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string AdminPassword = "AdPAss_123";
    }
}
