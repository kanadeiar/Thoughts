using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Thoughts.WebAPI.Clients.Test.Weather;

public class WeatherClient : IWeatherService
{
    private readonly IHttpClientFactory _httpFactory;

    public WeatherClient(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

    public async Task<IEnumerable<WeatherInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var client = _httpFactory.CreateClient("WebAPI");

        var result = await client
            .GetFromJsonAsync<IEnumerable<WeatherInfo>>("api/test/weather", cancellationToken)
            .ConfigureAwait(false);

        return result ?? Enumerable.Empty<WeatherInfo>();
    }

    public IEnumerable<WeatherInfo> GetAll() => GetAllAsync().GetAwaiter().GetResult();
}

public record WeatherInfo(DateTime Date, string? Summary)
{
    [JsonPropertyName("TemperatureC")]
    public int Temperature { get; init; }
}