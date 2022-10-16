using Microsoft.AspNetCore.Identity;

namespace Thoughts.DAL.Entities.Idetity
{
    public class IdentRole : IdentityRole
    {
        public const string Administrators = "Administrators";
        public const string Users = "Users";
    }
}
