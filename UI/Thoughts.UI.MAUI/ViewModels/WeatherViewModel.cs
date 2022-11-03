using System.Collections.ObjectModel;
using System.Windows.Input;
using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.UI.MAUI.ViewModels.Base;
using Thoughts.WebAPI.Clients.Test.Weather;

namespace Thoughts.UI.MAUI.ViewModels
{
    public class WeatherViewModel : ViewModel
    {
        private readonly IWeatherManager _weatherManager;

        #region Commands

        private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand ??= new Command(LoadData);

        #endregion

        public WeatherViewModel(IWeatherManager weatherManager)
        {
            _weatherManager = weatherManager;
            Title = "Погода";
        }

        public ObservableCollection<WeatherInfo> WeatherInfos { get; } = new();

        async void LoadData()
        {
            WeatherInfos.Clear();

            var infos = await _weatherManager.GetAllInfosAsync();

            foreach (var info in infos)
                WeatherInfos.Add(info);
        }
    }
}
