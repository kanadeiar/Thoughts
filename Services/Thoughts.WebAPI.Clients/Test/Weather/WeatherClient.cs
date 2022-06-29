using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Thoughts.WebAPI.Clients.Test.Weather;

public class WeatherClient
{
    private readonly HttpClient _Client;

    public WeatherClient(HttpClient Client) => _Client = Client;

    public async Task<IEnumerable<WeatherInfo>> GetAll()
    {
        var result = await _Client.GetFromJsonAsync<IEnumerable<WeatherInfo>>("api/test/weather");
        return result ?? throw new InvalidOperationException("Не удалось получить данные от сервиса");
    }
}

public record WeatherInfo(DateTime Date, string? Summary)
{
    [JsonPropertyName("TemperatureC")]
    public int Temperature { get; init; }
}