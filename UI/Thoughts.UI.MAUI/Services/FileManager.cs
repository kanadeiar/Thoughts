using Thoughts.UI.MAUI.Services.Interfaces;

namespace Thoughts.UI.MAUI.Services
{
    public class FileManager : IFileManager
    {
        public async Task<bool> UploadFileAsync(FileResult file, CancellationToken token = default)
        {
            return await Task.FromResult(true);
        }

        public bool UploadFile(FileResult file) => UploadFileAsync(file).GetAwaiter().GetResult();
    }
}
