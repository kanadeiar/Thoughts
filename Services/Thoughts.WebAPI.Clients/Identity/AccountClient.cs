using Microsoft.Extensions.Logging;

using Thoughts.DAL.Entities.Idetity;

namespace Thoughts.WebAPI.Clients.Identity;

public class AccountClient
{
    //private readonly ILogger<AccountClient> _Logger;

    public HttpClient Http { get; }

    public AccountClient(HttpClient Http/*, ILogger<AccountClient> Logger*/)
    {
        //_Logger = Logger;
        this.Http    = Http;
    }

    public async Task<List<IdentUser>> GetAllUsersAsync(CancellationToken Cancel = default)
    {
        throw new NotImplementedException();
    }
}
