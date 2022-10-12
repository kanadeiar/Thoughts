using System.Diagnostics;

using Identity.DAL;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Thoughts.DAL;
using Thoughts.DAL.Entities.Idetity;
using Thoughts.Services.Data;

using static System.Formats.Asn1.AsnWriter;

namespace Thoughts.Services.InSQL;

public class IdentityDbInitializer
{
    private readonly IdentityDB _db;
    private readonly UserManager<IdentUser> _userManager;
    private readonly RoleManager<IdentRole> _roleManager;
    private readonly ILogger<ThoughtsDbInitializer> _log;

    public IdentityDbInitializer(IdentityDB db, UserManager<IdentUser> userManager, RoleManager<IdentRole> roleManager, ILogger<ThoughtsDbInitializer> log)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _log = log;
    }

    public async Task DeleteAsync(CancellationToken Cancel = default)
    {
        await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);
    }

    public async Task InitializeAsync(bool RemoveBefore = false, bool InitializeTestData = false, CancellationToken Cancel = default)
    {
        if (RemoveBefore)
            await DeleteAsync(Cancel).ConfigureAwait(false);

        var pending_migrations = await _db.Database.GetPendingMigrationsAsync(Cancel).ConfigureAwait(false);
        var applied_migrations = await _db.Database.GetPendingMigrationsAsync(Cancel);

        if (applied_migrations.Any())
            _log.LogInformation("К БД применены миграции: {0}", string.Join(",", applied_migrations));

        if (pending_migrations.Any())
        {
            _log.LogInformation("Применение миграций: {0}...", string.Join(",", pending_migrations));
            await _db.Database.MigrateAsync(Cancel);
            _log.LogInformation("Применение миграций выполнено");
        }
        else
        {
            await _db.Database.EnsureCreatedAsync(Cancel);
        }

        if (InitializeTestData)
            await InitializeTestDataAsync(Cancel);
    }

    private async Task InitializeTestDataAsync(CancellationToken Cancel = default)
    {
        if (await _db.Users.AnyAsync(Cancel).ConfigureAwait(false))
        {
            _log.LogInformation("В базе данных есть пользователи - в инициализации тестовыми данными не нуждается");
            return;
        }

        var timer = Stopwatch.StartNew();

        string adminLogin = IdentUser.Administrator;
        string adminPassword = IdentUser.AdminPassword;


        if (await _roleManager.FindByNameAsync(IdentRole.Administrators) is null)
        {
            await _roleManager.CreateAsync(new IdentRole() { Name = IdentRole.Administrators });
        }

        if (await _roleManager.FindByNameAsync(IdentRole.Users) is null)
        {
            await _roleManager.CreateAsync(new IdentRole() { Name = IdentRole.Users });
        }

        if (await _userManager.FindByNameAsync(adminLogin) is null)
        {
            var admin = new IdentUser()
            {
                UserName = adminLogin
            };

            var identityResult = await _userManager.CreateAsync(admin, adminPassword);

            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, IdentRole.Administrators);
            }
        }

        _log.LogInformation("Инициализация БД тестовыми данными выполнена успешно за {0} мс", timer.ElapsedMilliseconds);
    }
}
