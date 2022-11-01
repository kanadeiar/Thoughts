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

        public async Task<bool> UploadLimitSizeFileAsync(FileResult file, CancellationToken token = default)
        {
            using var stream = await file.OpenReadAsync().ConfigureAwait(false);

            if(stream is null) return false;

            var result = await _filesService.UploadLimitSizeFileAsync(stream, file.FileName, file.ContentType, token).ConfigureAwait(false);

            return result;
        }

        public async Task<bool> UploadAnyFileAsync(FileResult file, CancellationToken token = default)
        {
            using var stream = await file.OpenReadAsync().ConfigureAwait(false);

            if (stream is null) return false;

            var result = await _filesService.UploadAnyFileAsync(stream, file.FileName, file.ContentType, token).ConfigureAwait(false);

            return result;
        }

        #region Sync versions

        public bool UploadLimitSizeFile(FileResult file) => UploadLimitSizeFileAsync(file).GetAwaiter().GetResult();

        public bool UploadAnyFile(FileResult file) => UploadAnyFileAsync(file).GetAwaiter().GetResult();

        #endregion

        #endregion
    }
}
