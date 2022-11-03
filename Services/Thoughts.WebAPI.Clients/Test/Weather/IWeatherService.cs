namespace Thoughts.WebAPI.Clients.Test.Weather
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherInfo>> GetAllAsync(CancellationToken cancellationToken = default);

        IEnumerable<WeatherInfo> GetAll();
    }
}
