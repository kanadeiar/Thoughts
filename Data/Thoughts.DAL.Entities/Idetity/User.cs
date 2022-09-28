using Microsoft.AspNetCore.Identity;

namespace Thoughts.DAL.Entities.Idetity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string AdminPassword = "AdPAss_123";

        /// <summary>Соль</summary>
        [Required, StringLength(100)]
        public string PasswordSalt { get; set; }
        public override string ToString() => UserName;
    }
}
