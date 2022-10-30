using Microsoft.Extensions.Logging;

using Thoughts.UI.MAUI.Services.Interfaces;
using Thoughts.WebAPI.Clients.Files;

namespace Thoughts.UI.MAUI.Services
{
    public class FileManager : IFileManager
    {
        #region Fields

        private readonly IFileService _filesService;
        private readonly ILogger<FileManager> _logger;

        #endregion

        #region Constructors

        public FileManager(IFileService filesService, ILogger<FileManager> logger)
        {
            _filesService = filesService;
            _logger = logger;
        }

        #endregion

        #region IFileManager implementation

        public async Task<bool> UploadFileAsync(FileResult file, CancellationToken token = default)
        {
            using var stream = await file.OpenReadAsync().ConfigureAwait(false);

            if(stream is null) return false;

            var result = await _filesService.UploadFileAsync(stream, token).ConfigureAwait(false);

            return result;
        }

        public bool UploadFile(FileResult file) => UploadFileAsync(file).GetAwaiter().GetResult(); 

        #endregion
    }
}
