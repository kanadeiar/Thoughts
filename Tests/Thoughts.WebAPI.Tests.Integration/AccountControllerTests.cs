using Microsoft.AspNetCore.Mvc.Testing;

using static WebStore.Interfaces.Services.WebAPIAddresses.Addresses.Identity;

using System.Net;

namespace Thoughts.WebAPI.Tests.Integration;

[TestClass]
public class AccountControllerTests
{
    private WebApplicationFactory<Program> _WebAPIHostBuilder;

    [TestInitialize]
    public void Initialize()
    {
        _WebAPIHostBuilder = new WebApplicationFactory<Program>()
            //.WithWebHostBuilder(builder => builder
            //.ConfigureServices(services =>
            //     {
            //         services.RemoveAll<ThoughtsDbInitializer>();

            //         services.RemoveAll(typeof(IRepository<>));
            //         services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            //     }))
            ;
    }

    [TestMethod]
    [DataRow("Admin", "AdPAss_123")]
    public async Task LoginFromService_GoodPasswordOrLogin(string login, string password)
    {
        var http_client = _WebAPIHostBuilder
        //.WithWebHostBuilder(builder => builder
        //    .ConfigureServices(services =>
        //     {
        //         services.RemoveAll(typeof(IRepository<>));
        //         services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
        //     }))
           .CreateClient();

        var response = await http_client.PostAsJsonAsync($"{Accounts}/Login", new { login, password });

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        response.Headers.TryGetValues("Authorization", out var text);
        Assert.IsNotNull(text);

        http_client.DefaultRequestHeaders.Authorization = new("Bearer", text?.FirstOrDefault());

        var roles = await http_client.GetAsync($"{Accounts}/GetAllRoles").ConfigureAwait(false); ;

        Assert.IsNotNull(roles);

        var users = await http_client.GetAsync($"{Accounts}/GetAllUsers").ConfigureAwait(false);

        Assert.IsNotNull(users);
    }

    [TestMethod]
    [DataRow("Admin", "BadPassword")]
    [DataRow("BadLogin", "AdPAss_123")]
    [DataRow("BadLogin", "BadPassword")]
    public async Task LoginFromService_BadPasswordOrLogin(string login, string password)
    {
        var http_client = _WebAPIHostBuilder
        //.WithWebHostBuilder(builder => builder
        //    .ConfigureServices(services =>
        //     {
        //         services.RemoveAll(typeof(IRepository<>));
        //         services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
        //     }))
           .CreateClient();

        var response = await http_client.PostAsJsonAsync($"{Accounts}/Login", new { login, password });

        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

        response.Headers.TryGetValues("Authorization", out var text);
        Assert.IsNull(text);

        http_client.DefaultRequestHeaders.Authorization = new("Bearer", text?.FirstOrDefault());

        var roles = await http_client.GetAsync($"{Accounts}/GetAllRoles").ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.Unauthorized, roles.StatusCode);

        var users = await http_client.GetAsync($"{Accounts}/GetAllUsers").ConfigureAwait(false);

        Assert.AreEqual(HttpStatusCode.Unauthorized, users.StatusCode);
    }
}
