using Microsoft.AspNetCore.Mvc.Testing;

using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.WebAPI.Tests.Integration;

[TestClass]
public class WeatherApiTests
{
    [TestMethod]
    public async Task ReceiveDataFromService()
    {
        var builder = new WebApplicationFactory<Program>();

        var http_client = builder.CreateClient();

        var weather_client = new WeatherClient(http_client);

        var weather = await weather_client.GetAll();
    }
}
