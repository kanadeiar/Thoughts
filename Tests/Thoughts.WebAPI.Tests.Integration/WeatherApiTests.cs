using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Thoughts.Interfaces.Base.Repositories;
using Thoughts.Services.InSQL;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.WebAPI.Tests.Integration;

[TestClass]
public class WeatherApiTests
{
    private WebApplicationFactory<Program> _WebAPIHostBuilder;

    [TestInitialize]
    public void Initialize()
    {
        _WebAPIHostBuilder = new WebApplicationFactory<Program>()
           //.WithWebHostBuilder(builder => builder
           //    .ConfigureServices(services =>
           //     {
           //         services.RemoveAll(typeof(IRepository<>));
           //         services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
           //     }))
            ;
    }

    [TestMethod]
    public async Task ReceiveDataFromService()
    {

        var http_client = _WebAPIHostBuilder
           //.WithWebHostBuilder(builder => builder
           //    .ConfigureServices(services =>
           //     {
           //         services.RemoveAll(typeof(IRepository<>));
           //         services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
           //     }))
           .CreateClient();

        var weather_client = new WeatherClient(http_client);

        var weather = await weather_client.GetAll();
    }
}
