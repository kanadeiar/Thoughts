using System.Windows.Input;

using Microsoft.Extensions.Logging;

using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.UI.MAUI.ViewModels.Base;

namespace Thoughts.UI.MAUI.ViewModels
{
    public class FileViewModel : ViewModel
    {
        #region Fields

        private readonly IFileManager _fileManager;
        private readonly IConnectivity _connectivity;
        private readonly ILogger<FileViewModel> _logger;

        private readonly PickOptions _pickOptions;

        #endregion

        #region Constructors

        public FileViewModel(IFileManager fileManager, 
            IConnectivity connectivity, 
            ILogger<FileViewModel> logger = default)
        {
            _fileManager = fileManager;
            _connectivity = connectivity;
            _logger = logger;

            Title = "Файлы";

            _pickOptions = new PickOptions
            {
                PickerTitle = "Выберите файл",
            };
        }

        #endregion

        #region Commands

        #region Upload image

        ICommand _uploadImageCommand;

        public ICommand UploadImageCommand => _uploadImageCommand ??= new Command(OnUploadImageAsync); 

        private async void OnUploadImageAsync()
        {
            if (IsBusy) return;

            try
            {
                await CheckInternetConnectionAsync();

                IsBusy = true;

                _pickOptions.FileTypes = FilePickerFileType.Images;

                var file = await FilePicker.Default.PickAsync(_pickOptions);

                if (file is null) return;

                var result = await _fileManager.UploadLimitSizeFileAsync(file);

                _logger?.LogInformation("{Method}: upload is {success}", nameof(OnUploadImageAsync), result);

                var resultMsg = result ? "успешно" : "с ошибкой";

                await Shell.Current.DisplayAlert("Загрузка файла",
                    $"Загрузка {file.FileName} завершилась {resultMsg}!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Method}: {message}", nameof(OnUploadImageAsync), ex.Message);
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to upload file: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Upload any file

        ICommand _uploadAnyCommand;

        public ICommand UploadAnyCommand => _uploadAnyCommand ??= new Command(OnUploadAnyAsync);

        private async void OnUploadAnyAsync()
        {
            if (IsBusy) return;

            try
            {
                await CheckInternetConnectionAsync();

                IsBusy = true;

                _pickOptions.FileTypes = default;

                var file = await FilePicker.Default.PickAsync(_pickOptions);

                if (file is null) return;

                var result = await _fileManager.UploadAnyFileAsync(file);

                _logger?.LogInformation("{Method}: upload is {success}", nameof(OnUploadAnyAsync), result);

                var resultMsg = result ? "успешно" : "с ошибкой";

                await Shell.Current.DisplayAlert("Загрузка файла",
                    $"Загрузка {file.FileName} завершилась {resultMsg}!", "OK");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Method}: {message}", nameof(OnUploadAnyAsync), ex.Message);
                await Shell.Current.DisplayAlert("Error!",
                    $"Unable to upload file: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #endregion

        #region Methods

        private async Task CheckInternetConnectionAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _logger?.LogError("{Method}: {message}", nameof(CheckInternetConnectionAsync), "Check your internet connection");
                await Shell.Current.DisplayAlert("Internet connection failed!",
                    $"Unable to upload file: Check your internet connection", "OK");
            }
        }

        #endregion
    }
}
