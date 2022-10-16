using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Identity.DAL.Interfaces;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

using Thoughts.DAL.Entities.Idetity;

namespace Thoughts.WebAPI.Services
{
    public class AuthUtils : IAuthUtils<IdentUser>
    {
        private readonly IConfiguration _configuration;
        private string _secretKey;

        public AuthUtils(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateSessionToken(IdentUser user, IList<string> roles)
        {
            var config = _configuration.GetSection("SecretTokenKey");
            _secretKey = config["Key"];

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims.ToArray()),

                Expires = DateTime.Now.AddMinutes(15),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);

        }
    }
}
