using Microsoft.Extensions.Logging;
using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.UI.MAUI.Services
{
    public class WeatherManager : IWeatherManager
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherManager> _logger;

        public WeatherManager(IWeatherService weatherService, ILogger<WeatherManager> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherInfo>> GetAllInfosAsync(CancellationToken cancellationToken = default)
        {
            var result = default(IEnumerable<WeatherInfo>);

            try
            {
                result = await _weatherService.GetAllAsync(cancellationToken)
                        .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Method}: {message}", nameof(GetAllInfosAsync), e.Message);
            }

            return result ?? Enumerable.Empty<WeatherInfo>();
        }

        public IEnumerable<WeatherInfo> GetAllInfos() => GetAllInfosAsync().GetAwaiter().GetResult();
    }
}
