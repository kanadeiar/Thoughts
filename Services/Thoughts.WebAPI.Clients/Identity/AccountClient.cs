using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;

using Microsoft.Extensions.Logging;

using Thoughts.DAL.Entities.Idetity;

using static WebStore.Interfaces.Services.WebAPIAddresses.Addresses.Identity;
using System.Threading;

namespace Thoughts.WebAPI.Clients.Identity;

public class AccountClient //: BaseClient //, UsersClient, IRolesClient
{
    private readonly ILogger<AccountClient> _Logger;
    public HttpClient Http { get; }

    public AccountClient(HttpClient Http, ILogger<AccountClient> Logger)
    {
        this.Http = Http;
        _Logger = Logger;
    }

    public async Task<List<IdentUser>?> GetAllUsersAsync(CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Accounts}/GetAllUsers", Cancel).ConfigureAwait(false);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.NotFound:
            case HttpStatusCode.Unauthorized:
                return default;
            default:
                var result = await response
                   .EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<List<IdentUser>>(cancellationToken: Cancel);
                return result;
        }
    }

    public async Task<List<IdentRole>?> GetAllRolessAsync(CancellationToken Cancel = default)
    {
        var response = await Http.GetAsync($"{Accounts}/GetAllRoles", Cancel).ConfigureAwait(false);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.NotFound:
            case HttpStatusCode.Unauthorized:
                return default;
            default:
                var result = await response
                   .EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<List<IdentRole>>(cancellationToken: Cancel);
                return result;
        }
    }

    public async Task LoginAsync(string login, string password, CancellationToken Cancel = default)
    {
        var response = await Http.PostAsJsonAsync($"{Accounts}/Login", new { login, password }).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var bearer = response.Headers.GetValues("Authorization").FirstOrDefault();
            Http.DefaultRequestHeaders.Authorization = new("Bearer", bearer);
        }
        else
            throw new InvalidOperationException("Не удалось получить токен от сервера! Авторизация не выполнена.");
    }

    public async Task LogoutAsync(CancellationToken Cancel = default)
    {
        var response = await Http.PostAsJsonAsync($"{Accounts}/Logout", Cancel).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
