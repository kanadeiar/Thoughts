using System.Net.Http.Json;
using System.Net;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using Thoughts.DAL.Entities.Idetity;
using Thoughts.Interfaces.Clients;
using Thoughts.WebAPI.Clients.Base;

using WebStore.Interfaces.Services;
using static WebStore.Interfaces.Services.WebAPIAddresses.Addresses.Identity;
using static System.Net.WebRequestMethods;
using System.Net.Http;

namespace Thoughts.WebAPI.Clients.Identity;

public class AccountClient //: BaseClient //, UsersClient, IRolesClient
{
    //private readonly ILogger<AccountClient> _Logger;
    //WebAPIAddresses.Addresses.Identity.Accounts
    public HttpClient Http { get; }

    public AccountClient(HttpClient Http/*, ILogger<AccountClient> Logger*/)
    {
        //_Logger = Logger;
        this.Http = Http;
    }

    public async Task<List<IdentUser>> GetAllUsersAsync(CancellationToken Cancel = default)
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

        //throw new NotImplementedException();
    }

    public async Task<List<IdentRole>> GetAllRolessAsync(CancellationToken Cancel = default)
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

        //throw new NotImplementedException();
    }
}
