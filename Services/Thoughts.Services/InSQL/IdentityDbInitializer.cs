using System.Diagnostics;

using Identity.DAL;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities.Idetity;
using Thoughts.Services.Data;

namespace Thoughts.Services.InSQL;

public class IdentityDbInitializer
{
    public static async Task InitializeAsync(UserManager<IdentUser> userManager, RoleManager<IdentRole> roleManager)
    {
        string adminLogin = IdentUser.Administrator;
        string adminPassword = IdentUser.AdminPassword;


        if (await roleManager.FindByNameAsync(IdentRole.Administrators) is null)
        {
            await roleManager.CreateAsync(new IdentRole() { Name = IdentRole.Administrators });
        }

        if (await roleManager.FindByNameAsync(IdentRole.Users) is null)
        {
            await roleManager.CreateAsync(new IdentRole() { Name = IdentRole.Users });
        }

        if (await userManager.FindByNameAsync(adminLogin) is null)
        {
            var admin = new IdentUser()
            {
                UserName = adminLogin
            };

            var identityResult = await userManager.CreateAsync(admin, adminPassword);

            if (identityResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, IdentRole.Administrators);
            }
        }
    }
}
