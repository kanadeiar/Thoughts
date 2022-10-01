using System.Collections.ObjectModel;
using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.UI.MAUI.ViewModels.Base;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.UI.MAUI.ViewModels
{
    public class WeatherViewModel : ViewModel
    {
        private readonly IWeatherManager _weatherManager;

        private string _title = "Weather";

        public string Title { get => _title; set => Set(ref _title, value); }

        public WeatherViewModel(IWeatherManager weatherManager)
        {
            _weatherManager = weatherManager;
            LoadData();
        }

        public ObservableCollection<WeatherInfo> WeatherInfos { get; } = new();

        private void LoadData()
        {
            WeatherInfos.Clear();

            var infos = _weatherManager.GetAllInfos();

            foreach (var info in infos)
                WeatherInfos.Add(info);
        }
    }
}
