using System.Windows.Input;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

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
        }

        #endregion

        #region Commands

        #region Upload file

        ICommand _uploadFileCommand;

        public ICommand UploadFileCommand => _uploadFileCommand ??= new Command(OnUploadFileAsync); 

        private async void OnUploadFileAsync()
        {
            if (IsBusy) return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    _logger?.LogError("{Method}: {message}", nameof(OnUploadFileAsync), "Check your internet connection");
                    await Shell.Current.DisplayAlert("Internet connection failed!",
                        $"Unable to upload file: Check your internet connection", "OK");
                }
                IsBusy = true;

                var file = await FilePicker.Default.PickAsync(PickOptions.Images);

                if (file is null) return;

                var result = await _fileManager.UploadFileAsync(file);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{Method}: {message}", nameof(OnUploadFileAsync), ex.Message);
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

        #endregion
    }
}
