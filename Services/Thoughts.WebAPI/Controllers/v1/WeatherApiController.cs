using Microsoft.AspNetCore.Mvc;

namespace Thoughts.WebAPI.Controllers.v1;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class WeatherApiController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherApiController> _logger;

    public WeatherApiController(ILogger<WeatherApiController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
