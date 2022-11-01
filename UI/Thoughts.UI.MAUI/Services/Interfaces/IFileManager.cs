namespace Thoughts.UI.MAUI.Services.Interfaces
{
    public interface IFileManager
    {
        Task<bool> UploadLimitSizeFileAsync(FileResult file, CancellationToken token = default);

        Task<bool> UploadAnyFileAsync(FileResult file, CancellationToken token = default);

        bool UploadLimitSizeFile(FileResult file);

        bool UploadAnyFile(FileResult file);
    }
}
