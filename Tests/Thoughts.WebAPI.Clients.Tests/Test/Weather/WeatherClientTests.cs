using System.Net;
using System.Net.Http.Json;

using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.WebAPI.Clients.Tests.Test.Weather;

[TestClass]
public class WeatherClientTests
{
    [TestMethod]
    public async Task GetAllTest()
    {
        HttpMessageHandler handler = new TestHttpMessageHandler(request =>
        {
            var content = JsonContent.Create(Enumerable.Range(1, 10).Select(i => new WeatherInfo(DateTime.Now, "Summary") { Temperature = 10 }));

            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };

            return Task.FromResult(response);
        });

        var http_client = new HttpClient(handler)
        {
            BaseAddress = new("http://server.ru")
        };

        var client = new WeatherClient(http_client);

        var result = await client.GetAllAsync();
    }
}

internal class TestHttpMessageHandler : HttpClientHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _Handler;

    public TestHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> Handler) => _Handler = Handler;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken Cancel)
    {
        //return base.SendAsync(request, Cancel);
        return _Handler(request);
    }
}
