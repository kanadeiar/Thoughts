using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Headers;

using Microsoft.Extensions.Logging;

using Thoughts.DAL.Entities.Idetity;

using static WebStore.Interfaces.Services.WebAPIAddresses.Addresses.Identity;

namespace Thoughts.WebAPI.Clients.Identity;

public class AccountClient //: BaseClient //, UsersClient, IRolesClient
{
    private readonly ILogger<AccountClient> _Logger;
    //WebAPIAddresses.Addresses.Identity.Accounts
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
                return default;
            default:
                var result = await response
                   .EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<List<IdentRole>>(cancellationToken: Cancel);
                return result;
        }
    }

    public async Task<string?> LoginAsync(string login, string password, CancellationToken Cancel = default)
    {
        //var requet   = new HttpRequestMessage(HttpMethod.Post, $"{Accounts}/Login?login={login}&password={password}");
        //var response = await Http.SendAsync(requet, Cancel).ConfigureAwait(false);

        var response = await Http.PostAsJsonAsync($"{Accounts}/Login", new { login, password }).ConfigureAwait(false);

        switch (response.StatusCode)
        {
            case HttpStatusCode.NoContent:
            case HttpStatusCode.NotFound:
                return default;
            default:
                //Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue();

                if (response.Headers.GetValues("Authorization").FirstOrDefault() is not { Length: > 10 } bearer)
                    throw new InvalidOperationException("Не удалось получить токен от сервера! Авторизация не выполнена.");

                Http.DefaultRequestHeaders.Authorization = new("Bearer", bearer[7..]);

                var result = await response
                   .EnsureSuccessStatusCode()
                   .Content
                   .ReadAsStringAsync(Cancel);
                return result;
        }
    }
}
